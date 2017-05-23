using KK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ParseGet
{
    class ShooterSubExtractor
    {
        string _outputDir = String.Empty;
        string _subFileNameWithoutExt = String.Empty;
        readonly Dictionary<string, int> _extCounter;
        string _videoFilePath = String.Empty;
        string _videoFileName = String.Empty;
        string _dumpFilePath = String.Empty;

        public enum SubExtractResult
        {
            OK,
            Error,
            NoSubFound
        }

        public ShooterSubExtractor()
        {
            _extCounter = new Dictionary<string, int>();
        }

        public string VideoFilePath
        {
            get { return _videoFilePath; }
            set {
                _videoFilePath = value;
                _outputDir = Path.GetDirectoryName(_videoFilePath);
                _videoFileName = Path.GetFileName(_videoFilePath);
                int dotIndex = _videoFileName.LastIndexOf('.');
                _subFileNameWithoutExt = _videoFileName.Substring(0, dotIndex);
            }
        }

        public string DumpFilePath
        {
            get { return _dumpFilePath; }
            set { _dumpFilePath = value; }
        }

        public SubExtractResult ExtractSubtitles()
        {
            SubExtractResult result = SubExtractResult.OK;
            FileStream subTempStream = null;
            BinaryReader subTempReader = null;
            try
            {
                subTempStream = new FileStream(_dumpFilePath, FileMode.Open, FileAccess.Read);
                subTempReader = new BinaryReader(subTempStream);
                var statCode = (SByte)subTempReader.ReadByte();
                bool bOk = true;
                if (statCode < 0)
                {
					result = statCode == -1 ? SubExtractResult.NoSubFound : SubExtractResult.Error;

                    bOk = false;
                }

                if (bOk)
                {
                    for (int i = 0; i < statCode; i++)
                    {
                        HandleSubPackage(subTempReader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _videoFileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = SubExtractResult.Error;
            }
            finally
            {
                if (subTempReader != null)
                    subTempReader.Close();
            }

            return result;
        }

        void HandleSubPackage(BinaryReader reader)
        {
            //TODO: Handle errors
            //package header
            //Can't use BinaryReader.ReadInt32(). 
            //The ReadInt32 reads value in LE order, while the value is encoded in BE order.
            //int packageLen = reader.ReadInt32();
            int packageLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);
            int descLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);
            byte[] desc = reader.ReadBytes(descLen);

            //file data header
            int fileDataLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);
            var numOfFiles = (SByte)reader.ReadByte();

            for (int i = 0; i < numOfFiles; i++)
            {
                HandleSingleSub(reader);
            }
        }

        void HandleSingleSub(BinaryReader reader)
        {
            //TODO: Handle errors
            int singleFilePackLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);
            int extLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);
            byte[] ext = reader.ReadBytes(extLen);
            string extString = Encoding.UTF8.GetString(ext);
            int fileLen = Util.BytesToInt32(reader.ReadBytes(4), ByteOrder.BigEndian);

            int leftToRead = fileLen;
            const int BufferSize = 4096;
            var buffer = new byte[BufferSize];
            string tempFilePath = Path.GetTempFileName();
            var tempStream =
                new FileStream(tempFilePath, FileMode.Open, FileAccess.ReadWrite);
            bool bGzipped = false;

            do
            {
                int needToRead = Math.Min(leftToRead, BufferSize);
                int accuRead = reader.Read(buffer, 0, needToRead);
                
                if (leftToRead == fileLen) //if it's the first round.
                {
                    //check if the data is gzipped.
                    bGzipped = (buffer[0] == 0x1f) && (buffer[1] == 0x8b) && (buffer[2] == 0x08);
                }

                if (accuRead == 0)
                {
                    break;
                }
                leftToRead -= accuRead;

                //Output
                tempStream.Write(buffer, 0, accuRead);
            } while (leftToRead > 0);
            tempStream.Close();

            bool bOk = true;
            //decompress
            if (bGzipped)
            {
                string flatTempFilePath = Path.GetTempFileName();
                bool ret = Util.UnGZip(tempFilePath, flatTempFilePath);
                File.Delete(tempFilePath);
                if (!ret)
                {
                    File.Delete(flatTempFilePath);
                    bOk = false;
                }
                else
                {
                    //continue processing the decompressed file.
                    tempFilePath = flatTempFilePath;
                }      
            }

            if (bOk)
            {
                //final output
                File.Move(tempFilePath, GetOutputFilename(extString));
            }
        }

        string GetOutputFilename(string ext)
        {
            const string LangId = "chn";
            int count;
            string outputPath;

            if (!_extCounter.TryGetValue(ext, out count))
            {
                outputPath = String.Format("{0}\\{1}.{2}.{3}",
                _outputDir, _subFileNameWithoutExt, LangId, ext);
                count = 1;
                _extCounter.Add(ext, count);
            }
            else
            {
                outputPath = String.Format("{0}\\{1}.{2}{3}.{4}",
                    _outputDir, _subFileNameWithoutExt, LangId, count, ext);
                count++;
                _extCounter[ext] = count;
            }

            //if the generated file name already exists, gen a new one
            while (File.Exists(outputPath))
            {
                outputPath = String.Format("{0}\\{1}.{2}{3}.{4}",
                    _outputDir, _subFileNameWithoutExt, LangId, count, ext);
                count++;
                _extCounter[ext] = count;
            }

            return outputPath;
        }
    }
}
