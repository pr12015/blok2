using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class DataBase
    {
        private static readonly string _dbPath = @"C:\Users\stefan\Desktop\blok22\db.txt";

        /// <summary>
        /// Write EMeter to DB.
        /// </summary>
        /// <param name="eMeter"> EMeter object to be written to DB. </param>
        public static void Write(EMeter eMeter)
        {
            // Check if EMeter with same ID already exists.
            if (Read(eMeter.ID) != null)
                throw new Exception("EMeter with ID = " + eMeter.ID + " already exists.");

            using (var writer = new StreamWriter(_dbPath, true))
            {
                writer.WriteLine(eMeter.ToString());
            }
        }

        /// <summary>
        /// Finds the EMeter with id in DB.
        /// </summary>
        /// <param name="id"> EMeter ID. </param>
        /// <returns> EMeter object read from DB. </returns>
        public static EMeter Read(int id)
        {
            string line;
            bool found = false;
            using (var reader = new StreamReader(_dbPath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var meterIDStr = line.Split(' ')[0];
                    var meterID = Int32.Parse(meterIDStr);
                    if (meterID == id)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (found)
                return EMeter.FromString(line);

            return null;
        }

        /// <summary>
        /// Modifies the ID of EMeter in Database.
        /// </summary>
        /// <param name="newID"> New value of ID field. </param>
        /// <param name="eMeterOld"> EMeter object read from DB. </param>
        public static void WriteModified(int newID, EMeter eMeterOld)
        {
            string destination = @"C:\Users\stefan\Desktop\blok22\db_tmp.txt";
            string line = null;

            using (var reader = new StreamReader(_dbPath))
            using (var writer = new StreamWriter(destination))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var lineContent = line.Split(' ');
                    int currentReading = Int32.Parse(lineContent[0]);

                    if (currentReading == eMeterOld.ID)
                    {
                        // Modify the eMeterOld and write to file.
                        eMeterOld.ID = newID;
                        writer.WriteLine(eMeterOld.ToString());
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            // Delete old db.txt and create new.
            File.Delete(_dbPath);
            File.Move(destination, _dbPath);
        }

        /// <summary>
        /// Modifies the Reading of EMeter in Database.
        /// </summary>
        /// <param name="newReading"> New value of Reading field. </param>
        /// <param name="eMeterOld"> EMeter object read from DB. </param>
        public static void WriteModified(double newReading, EMeter eMeterOld)
        {
            string destination = @"C:\Users\stefan\Desktop\blok22\db_tmp.txt";
            string line = null;

            using (var reader = new StreamReader(_dbPath))
            using (var writer = new StreamWriter(destination))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var lineContent = line.Split(' ');
                    double currentReading = Double.Parse(lineContent[2]);

                    if (currentReading == eMeterOld.Reading)
                    {
                        // Modify the eMeterOld and write to file.
                        eMeterOld.Reading = newReading;
                        writer.WriteLine(eMeterOld.ToString());
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            // Delete old db.txt and create new. Delete db_tmp.txt.
            File.Delete(_dbPath);
            File.Move(destination, _dbPath);
            File.Delete(destination);
        }

        /// <summary>
        /// Finds the EMeter with id and changes its id.
        /// </summary>
        /// <param name="id"> EMeter ID.</param>
        /// <param name="newID"> Modified ID. </param>
        public static void ModifyID(int id, int newID)
        {
            var eMeterOld = Read(id);

            if (eMeterOld != null)
                WriteModified(newID, eMeterOld);
            else
                throw new Exception("EMeter with ID: " + id + " doesn't exist.");
        }

        /// <summary>
        /// Finds the EMeter with id and modifies the Reading field.
        /// </summary>
        /// <param name="id"> EMeter ID. </param>
        /// <param name="newReading"> Modified reading. </param>
        public static void ModifyReading(int id, double newReading)
        {
            var eMeterOld = Read(id);

            if (eMeterOld != null)
                WriteModified(newReading, eMeterOld);
            else
                throw new Exception("EMeter with ID: " + id + " doesn't exist.");

        }
    }
}