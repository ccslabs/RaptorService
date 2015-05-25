using ContentObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaptorContentObjectsSeeder
{
    public partial class Form1 : Form
    {
        private RaptorService.RaptorServiceClient RAPI = new RaptorService.RaptorServiceClient();
        private NetTcpBinding binding;
        private ChannelFactory<RaptorStreamingHost> factory;
        private RaptorStreamingHost service;

        private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        PictureBox pbFaces;

        public Form1()
        {
            InitializeComponent();
        }

        Queue<string> ImagesQueue = new Queue<string>();
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            DialogResult dr = OFD.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Progress.Maximum = OFD.FileNames.Count();
                foreach (string filename in OFD.FileNames)
                {
                    try
                    {
                        ImagesQueue.Enqueue(filename);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                RunQueue();
                // Now let the user select the faces from the image
                // Then when the user clicks Save Faces - StreamToService and re-run RunQueue();
            }
        }

        private void RunQueue()
        {
            pbOriginal.Image = Image.FromFile(ImagesQueue.Dequeue());
            pbOriginal.MouseDown += PbOriginal_MouseDown;
            pbOriginal.MouseMove += PbOriginal_MouseMove;
            pbOriginal.Paint += PbOriginal_Paint;
            pbOriginal.MouseUp += PbOriginal_MouseUp;
            Progress.Value = ImagesQueue.Count();
        }

        private void PbOriginal_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void PbOriginal_Paint(object sender, PaintEventArgs e)
        {
            if (pbOriginal.Image != null)
            {
                if (Rect != null && Rect.Width > 0 && Rect.Height > 0)
                {
                    e.Graphics.FillRectangle(selectionBrush, Rect);
                }
            }
        }

        private void PbOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            Point tempEndPoint = e.Location;
            Rect.Location = new Point(
                Math.Min(RectStartPoint.X, tempEndPoint.X),
                Math.Min(RectStartPoint.Y, tempEndPoint.Y));
            Rect.Size = new Size(
                Math.Abs(RectStartPoint.X - tempEndPoint.X),
                Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            pbOriginal.Invalidate();
        }

        private void PbOriginal_MouseDown(object sender, MouseEventArgs e)
        {
            RectStartPoint = e.Location;
            Invalidate();
        }

        private void btnSaveFaces_Click(object sender, EventArgs e)
        {

        }

        private void StreamContentObjectToService(string filename)
        {
            ContentObjectData co = new ContentObjectData();
            

            co.Data = LoadFile(filename);
            co.Hash = Hashing.HashString(co.Data);
            co.DisplayImage = co.Data;
            co.FileName = co.Hash;
            Stream stream = SerializeToStream(co);
            service.SendContentObject(stream);

            //Rerun RunQueue()
            RunQueue();
        }

        private bool SendContentObject(Stream stream)
        {
            service.SendContentObject(stream);

            return true;
        }

        private string LoadFile(string filename)
        {
            return Hashing.BytesToString(File.ReadAllBytes(filename));
        }


        /// <summary>
        /// serializes the given object into memory stream
        /// </summary>
        /// <param name="objectType">the object to be serialized</param>
        /// <returns>The serialized object as memory stream</returns>
        public static Stream SerializeToStream(object objectType)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectType);
            stream.Position = 0L; // REMEMBER to reset stream or WCF will just send the stream from the end resulting in an empty stream!
            return (Stream)stream;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            try
            {
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ContentObjectData DeserializeFromStream(byte[] allBytes)
        {
            using (var stream = new MemoryStream(allBytes))
                return (ContentObjectData)DeserializeFromStream(stream);
        }

        public static ContentObjectData DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            ContentObjectData objectType = (ContentObjectData)formatter.Deserialize(stream);
            return objectType;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (RAPI.Login(Properties.Settings.Default.Username, Properties.Settings.Default.Password))
            {
                binding = new NetTcpBinding(SecurityMode.None, false);
                // TimeSpan.MaxValue is interpreted with a rounding error, so use 24 days instead
                binding.SendTimeout = TimeSpan.FromDays(24);
                binding.TransferMode = TransferMode.Streamed;
                binding.MaxReceivedMessageSize = 9223372036854775807;
                factory = new ChannelFactory<RaptorStreamingHost>(binding, new EndpointAddress("net.tcp://ccs-labs.com:804/"));
                service = factory.CreateChannel();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RAPI.LogOff(); // Log the user out nicely
            ((IClientChannel)service).Close();
        }

        private void btnLoadFromDatabase_Click(object sender, EventArgs e)
        {
            // lets get ten at a time

            for (int idx = 0; idx < 10; idx++)
            {
                PictureBox pb = new PictureBox();

                Stream stream = service.GetSpecificContentObject(idx);
                MemoryStream memStream = new MemoryStream();
                stream.CopyTo(memStream);
                byte[] alBytes = memStream.ToArray();
                stream.Close();
                ContentObjectData cod = DeserializeFromStream(alBytes);
                MemoryStream memStream2 = new MemoryStream(Hashing.StringToBytes(cod.DisplayImage));
                pb.Image = Image.FromStream(memStream2);
                pb.Size = new Size(100, 100);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Name = cod.FileName; // Stupd Filenames at the moment.
                pb.Tag = idx;
                flowLayoutPanel.Controls.Add(pb);
            }
        }

        private void btnAddFaces_Click(object sender, EventArgs e)
        {
            // Convert the pbFaces control contents to the stream and StreamToService.

            Bitmap sourceBitmap = new Bitmap(pbOriginal.Image);
            Bitmap croppedBitmap = sourceBitmap.Clone(Rect, sourceBitmap.PixelFormat);
            pbFaces = new PictureBox();
            pbFaces.Image = croppedBitmap;
            pbFaces.SizeMode = PictureBoxSizeMode.AutoSize;
            flowLayoutPanelRight.Controls.Add(pbFaces);

        }

       
    }

    [ServiceContract]
    public interface RaptorStreamingHost
    {
        [OperationContract]
        void SendContentObject(Stream data);

        [OperationContract]
        Stream GetContentObject();

        [OperationContract]
        Stream GetSpecificContentObject(int ContentNumber);
    }

    internal class Hashing
    {
        internal static string HashString(string password)
        {
            byte[] hash = null;
            var data = Encoding.UTF8.GetBytes(password);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(data);
            }

            return BytesToString(hash);
        }

        internal static string BytesToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "").ToUpper();
        }

        internal static byte[] StringToBytes(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        //public static string HashString(string text)
        //{
        //    text = text.ToLowerInvariant().Trim().ToString(); // Ensure that hashing a particular text is always from the same base

        //    string hash = "";
        //    SHA512 alg = SHA512.Create();
        //    byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(text));
        //    hash = Encoding.UTF8.GetString(result);
        //    return hash;
        //}
    }


}
