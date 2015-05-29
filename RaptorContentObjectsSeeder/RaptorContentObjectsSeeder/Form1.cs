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
        #region Setup and Globals
        private RaptorService.RaptorServiceClient RAPI = new RaptorService.RaptorServiceClient();
        private NetTcpBinding binding;
        private ChannelFactory<RaptorStreamingHost> factory;
        private RaptorStreamingHost service;

        private Rectangle Rect = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        PictureBox pbFaces;
        #endregion


        public Form1()
        {
            InitializeComponent();
            
        }


        #region Load Images 

        // Generate a Display Image from all the faces extracted from the Original Image
        private string LoadFacesFile()
        {
            // We know that flowpanelright only contains pictureboxes of faces 
            // so that makes things easy for us to get all the faces

            int idx = 1;
            foreach (PictureBox pb in flowLayoutPanelRight.Controls)
            {
                pb.Image.Save(idx + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                idx++;
            }


            // Save the faces to disk for tghe CombineBitmap method
            string[] filenames = new string[idx - 1];
            for (int idx2 = 1; idx2 < idx; idx2++)
            {
                filenames[idx2 - 1] = idx2 + ".bmp";
            }

            // combine the faces into a single Bitmap
            Bitmap combinedFaces = CombineBitmap(filenames);

            // Delete All the Saved Faces on Disk 
            // Would be better if all this was done in memory!
            for (int idx3 = 1; idx3 < idx; idx3++)
            {
                File.Delete(idx3 + ".bmp");
            }

            combinedFaces.Save("combinedFaces.bmp");        // Save this for loading and converting
            string res = LoadFile("combinedFaces.bmp");     // Convert to Byte[]
            File.Delete("CombinedFaces.bmp");               // Delete this as it is no longer needed
            combinedFaces.Dispose();                        // Clear up Resource Useage
            filenames = null;                               // Clear up Resource Useage
            return res;                                     // Return the byte array.
        }

        private string LoadFile(string filename)
        {
            return Hashing.ToHex(File.ReadAllBytes(filename));
        }

        private string currentImage = null;
        private void RunQueue()
        {
            GC.Collect();
            currentImage = ImagesQueue.Dequeue();
            FileInfo finfo = new FileInfo(currentImage);
            lblFileSize.Text = (finfo.Length / 1024).ToString("N0");
            pbOriginal.Image = Image.FromFile(currentImage);

            Progress.Value = ImagesQueue.Count();
        }
        #endregion

        #region PictureBox Events

        private void pbOriginal_MouseDown_1(object sender, MouseEventArgs e)
        {
            // RectStartPoint = e.Location;
            mDown = e.Location;
            Invalidate();
        }

        private void pbOriginal_MouseMove_1(object sender, MouseEventArgs e)
        {
            //if (e.Button != MouseButtons.Left)
            //    return;
            //Point tempEndPoint = e.Location;
            //Rect.Location = new Point(
            //    Math.Min(RectStartPoint.X, tempEndPoint.X),
            //    Math.Min(RectStartPoint.Y, tempEndPoint.Y));
            //Rect.Size = new Size(
            //    Math.Abs(RectStartPoint.X - tempEndPoint.X),
            //    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            if (e.Button != MouseButtons.Left)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mCurr = e.Location;
                pbOriginal.Invalidate();
            }

            //  pbOriginal.Invalidate();
        }

        private void pbOriginal_Paint_1(object sender, PaintEventArgs e)
        {
            //if (pbOriginal.Image != null)
            //{
            //    if (Rect != null && Rect.Width > 0 && Rect.Height > 0)
            //    {
            //        e.Graphics.DrawRectangle(Pens.AliceBlue, Rect);
            //    }
            //}
            if (pbOriginal.Image != null)
            {
                Rectangle r = new Rectangle(mDown.X, mDown.Y, mCurr.X - mDown.X, mCurr.Y - mDown.Y);
                e.Graphics.DrawRectangle(Pens.Orange, r);
                pbOriginal.Invalidate();
            }
        }

        #endregion

        #region ImageManipulation

        public static System.Drawing.Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
                images = null;
            }
        }

        #endregion

        #region ContentObject Actions

        private string coHash = null;
        private bool StreamContentObjectToService(string filename)
        {
            ContentObjectData co = new ContentObjectData();

            co.Data = LoadFile(filename);
            coHash = Hashing.HashString(co.Data); ;
            co.Hash = coHash;
            co.DisplayImage = LoadFacesFile();
            co.FileName = co.Hash;
            Stream stream = SerializeObjectToStream(co);
            return service.SendContentObject(stream);
        }

        #endregion

        #region Faces Actions
        private void StreamFacesToService()
        {
            foreach (PictureBox pb in flowLayoutPanelRight.Controls)
            {
                FacesLib.FacesObject face = new FacesLib.FacesObject();
                face.COFace = LoadFile((string)pb.Tag); // The filename to process.
                face.COId = service.GetContentObjectId(coHash);
                Stream stream = SerializeObjectToStream(face);
                service.SendFaces(stream);
                File.Delete((string)pb.Tag); // This is the tmp file created
            }
        }

        #endregion

        #region Form Events

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

        #endregion

        #region Button Events

        private void btnSkipImage_Click(object sender, EventArgs e)
        {
            flowLayoutPanelRight.Controls.Clear();
            if (ImagesQueue.Count > 0)
                RunQueue();
        }

        private void btnSaveFaces_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanelRight.Controls.Count > 0)
            {
                if (StreamContentObjectToService(currentImage))
                    StreamFacesToService();
                flowLayoutPanelRight.Controls.Clear();

                if (ImagesQueue.Count > 0)
                    RunQueue();
            }
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
                        throw;
                    }
                }
                RunQueue();
                // Now let the user select the faces from the image
                // Then when the user clicks Save Faces - StreamToService and re-run RunQueue();
            }
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
                ContentObjectData cod = DeserializeContentObjectFromStream(alBytes);
                MemoryStream memStream2 = new MemoryStream(Hashing.StringToBytes(cod.DisplayImage));
                pb.Image = Image.FromStream(memStream2);
                pb.Size = new Size(100, 100);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Name = cod.FileName; // Stupd Filenames at the moment.
                pb.Tag = idx;
                flowLayoutPanel.Controls.Add(pb);
            }
        }

        private void btnRemoveLastFace_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanelRight.Controls.Count > 0)
                flowLayoutPanelRight.Controls.RemoveAt(flowLayoutPanelRight.Controls.Count - 1);
            flowLayoutPanelRight.Invalidate();
            flowLayoutPanelRight.Refresh();
        }

        Point mDown = Point.Empty;
        Point mCurr = Point.Empty;
        private void btnAddFaces_Click(object sender, EventArgs e)
        {

            if (pbOriginal.Image != null)
            {
                float stretch1X;
                float stretch1Y;
                Point pt = new Point();
                Size sz = new Size();


                stretch1X = 1f * pbOriginal.Image.Width / pbOriginal.ClientSize.Width;
                stretch1Y = 1f * pbOriginal.Image.Height / pbOriginal.ClientSize.Height;

                pt = new Point((int)(mDown.X * stretch1X), (int)(mDown.Y * stretch1Y));
                sz = new Size((int)((mCurr.X - mDown.X) * stretch1X),
                                   (int)((mCurr.Y - mDown.Y) * stretch1Y));



                Rectangle rSrc = new Rectangle(pt, sz);
                Rectangle rDest = new Rectangle(Point.Empty, sz);
                if (sz.Width != 0 || sz.Height != 0)
                {
                    Bitmap bmp = new Bitmap(sz.Width, sz.Height);

                    // Really unlikely to have two rectangles in the exact same position !!
                    string fname = rSrc.X.ToString() + rSrc.Y.ToString() + rSrc.Height.ToString() + rSrc.Width.ToString() + ".bmp";
                    bmp.Save(fname);
                    pbFaces = new PictureBox();
                    pbFaces.Image = bmp;
                    pbFaces.BorderStyle = BorderStyle.FixedSingle;
                    pbFaces.Size = new Size(210, 143);

                    // pbFaces.Image = croppedBitmap;
                    using (Graphics G = Graphics.FromImage(bmp))
                        G.DrawImage(pbOriginal.Image, rDest, rSrc, GraphicsUnit.Pixel);
                    pbFaces.Image = bmp;

                    pbFaces.SizeMode = PictureBoxSizeMode.Zoom;
                    pbFaces.Tag = fname;
                    flowLayoutPanelRight.Controls.Add(pbFaces);
                    pbFaces.Invalidate();
                    pbFaces.Update();
                }
                else
                {
                    MessageBox.Show("You need to select a face or skip this image.");
                }
            }
            else
            {
                MessageBox.Show("You need to load Select an image first: Seed Database Button");
            }
        }

        #endregion

        #region Stream Management

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        #endregion

        #region Serialize Objects

        private static Stream SerializeObjectToStream(object objectType)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectType);
            stream.Position = 0L; // REMEMBER to reset stream or WCF will just send the stream from the end resulting in an empty stream!
            return (Stream)stream;
        }

        #endregion

        #region DeSerialize Objects

        private static FacesLib.FacesObject DeserializeFaceFromStream(byte[] allBytes)
        {
            using (var stream = new MemoryStream(allBytes))
                return (FacesLib.FacesObject)DeserializeFaceFromStream(stream);
        }

        private static FacesLib.FacesObject DeserializeFaceFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            FacesLib.FacesObject objectType = (FacesLib.FacesObject)formatter.Deserialize(stream);
            return objectType;
        }

        private static ContentObjectData DeserializeContentObjectFromStream(byte[] allBytes)
        {
            using (var stream = new MemoryStream(allBytes))
                return (ContentObjectData)DeserializeContentObjectFromStream(stream);
        }

        private static ContentObjectData DeserializeContentObjectFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            ContentObjectData objectType = (ContentObjectData)formatter.Deserialize(stream);
            return objectType;
        }





        #endregion

        private void flowLayoutPanelRight_Scroll(object sender, ScrollEventArgs e)
        {
            pbFaces.Invalidate();
            pbFaces.Update();
            flowLayoutPanelRight.Invalidate();
            flowLayoutPanelRight.Update();

        }

        private void timerPinger20Mins_Tick(object sender, EventArgs e)
        {

           if(!RAPI.Ping())
            {
                RAPI.Open();
            }
        }
    }


    // This is required as we do not have a MEX binding for this Service

    [ServiceContract]
    public interface RaptorStreamingHost
    {
        [OperationContract]
        bool SendContentObject(Stream data);

        [OperationContract]
        Stream GetContentObject();

        [OperationContract]
        Stream GetSpecificContentObject(int ContentNumber);

        [OperationContract]
        void SendFaces(Stream data);

        [OperationContract]
        long GetContentObjectId(string coHash);
    }

    internal static class Hashing
    {

        internal static readonly string[] HexStringTable = new string[]
{
    "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
    "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
    "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
    "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
    "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
    "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
    "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
    "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
    "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
    "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
    "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
    "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
    "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
    "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
    "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
    "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
};

        /// <summary>
        /// Returns a hex string representation of an array of bytes.
        /// </summary>
        /// <param name="value">The array of bytes.</param>
        /// <returns>A hex string representation of the array of bytes.</returns>
        internal static string ToHex(this byte[] value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (value != null)
            {
                foreach (byte b in value)
                {
                    stringBuilder.Append(HexStringTable[b]);
                }
            }

            return stringBuilder.ToString();
        }

        internal static string HashString(string password)
        {
            byte[] hash = null;
            var data = Encoding.UTF8.GetBytes(password);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(data);
            }

            return ToHex(hash);
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
