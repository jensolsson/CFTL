using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Drawing;

using System.Diagnostics;

namespace CyberForensicsTimeLabTest
{
    public class EvidenceObject : IComparable
    {   
        private string type;
        private string title;
        private string eventId;
        private string parentId;
        public EvidenceObject parent;

        private List<string> names = new List<string>();
        private List<string> values = new List<string>();

        private List<Int64> chunkFrom = new List<Int64>();
        private List<Int64> chunkTo = new List<Int64>();


        public EvidenceObject(string newTitle, string newType, string newEventId, EvidenceObject newParent)
        {   
            type = newType;
            title = newTitle;
            eventId = newEventId;
            if (newParent == null)
                parentId = "";
            else
                parentId = newParent.eventId;
            parent = newParent;
        }

        public int CompareTo(object obj)
        {
            EvidenceObject other = (EvidenceObject)obj;

            int value = title.CompareTo(other.title);
            if (value != 0)
                return value;

            value = eventId.CompareTo(other.eventId);
            if (value != 0)
                return value;

            return 0;
        }

        public string GetSourceType()
        {
            return type;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetEventId()
        {
            return eventId;
        }

        public string GetParentId()
        {
            return parentId;
        }

        public void addProperty(string name, string value)
        {
            names.Add(name);
            values.Add(value);
        }

        public void addChunk(Int64 from, Int64 to)
        {
            chunkFrom.Add(from);
            chunkTo.Add(to);
        }

        public string getName(int index)
        {
            if (index >= names.Count)
                return null;
            return names[index];
        }

        public string getValue(int index)
        {
            if (index >= values.Count)
                return null;
            return values[index];
        }

        public Int64 Size()
        {
	        Int64 size = 0;
            for(int i = 0; i<chunkFrom.Count; i++)
            {
                Int64 chunkLength = chunkTo[i] - chunkFrom[i];
                size += chunkLength;
            }
	        return size;	
        }

        public byte[] Data(Int64 start, Int64 length)
        { 
        	byte[] data = new byte[0];

            for(int i=0; i<chunkFrom.Count; i++)
            {
                Int64 chunkStart = chunkFrom[i];
                Int64 chunkEnd = chunkTo[i];
                Int64 chunkLength = chunkEnd - chunkStart;

	            if(start >= chunkLength) {
		            start -= chunkLength;
	            }
	            else {
		            Int64 amount = chunkLength - start;
                    if (length < amount)
                        amount = length;
                    byte[] buffer;
		            if(parent == null) {
	                    FileInfo file = new FileInfo(title);
                        FileStream fs = file.OpenRead();
                                                
                        buffer = new byte[amount];
                        fs.Position = chunkStart + start;
                        fs.Read(buffer, 0, (Int32)amount);
                        fs.Close();
		            }
		            else {
			            buffer = parent.Data(chunkStart + start, amount);
		            }
                    byte[] newData = new byte[data.Length + buffer.Length];
                    data.CopyTo(newData, 0);
                    buffer.CopyTo(newData, data.Length);

                    data = newData;
		            //data += buffer;
		            length -= amount;
	            }
            }
        	
            return data;
        }
    }


    public class Timestamp : IComparable
    {
        private long timestamp;
        private string eventType;
        private string timestampOrigin;
        private EvidenceObject evidenceObject;

        public Timestamp(string newType, long newTimestamp, string newTimestampOrigin, EvidenceObject newEvidenceObject)
        {
            eventType = newType;
            timestamp = newTimestamp;
            timestampOrigin = newTimestampOrigin;
            evidenceObject = newEvidenceObject;
        }

        public int CompareTo(object obj)
        {
            Timestamp other = (Timestamp)obj;

            int value = timestamp.CompareTo(other.timestamp);
            if (value != 0)
                return value;

            value = eventType.CompareTo(other.eventType);
            if (value != 0)
                return value;

            value = evidenceObject.CompareTo(other.evidenceObject);
            if (value != 0)
                return value;

            return 0;
        }

        public long GetTimestamp()
        {
            return timestamp;
        }

        public string GetTimestampOrigin()
        {
            return timestampOrigin;
        }

        public string GetEventType()
        {
            return eventType;
        }

        public override string ToString() {
            return timestamp.ToString();
        }

        public EvidenceObject EvidenceObject() {
            return evidenceObject;
        }

    }

    public class XmlDatabase
    {
        private ArrayList evidences = new ArrayList();
        private ArrayList timestamps = new ArrayList();

        private DateTime firstTimestampConstraint;
        private DateTime lastTimestampConstraint;

        private ArrayList distrubutionCacheKey = new ArrayList();
        private ArrayList distributionCacheItem = new ArrayList();

        public EvidenceObject GetEvidenceObject(int id)
        {
            return (EvidenceObject)evidences[id - 1];
        }

        public ArrayList GetTimestampsBetween(DateTime from, DateTime to)
        {
            ArrayList retTimestamps = new ArrayList();

            Boolean store = false;
            for (int i = 0; i < timestamps.Count; i++)
            {
                DateTime current = new DateTime(((Timestamp)timestamps[i]).GetTimestamp());
                if (current >= from)
                    store = true;
                if (current > to)
                    store = false;

                if (store)
                    retTimestamps.Add(timestamps[i]);
            }

            return retTimestamps;
        }

        public void SetFirstTimeStamp(DateTime timestamp)
        {
            firstTimestampConstraint = timestamp;
            distrubutionCacheKey.Clear();
            distributionCacheItem.Clear();
        }

        public void SetLastTimeStamp(DateTime timestamp)
        {
            lastTimestampConstraint = timestamp;
            distrubutionCacheKey.Clear();
            distributionCacheItem.Clear();
        }

        public DateTime FirstTimeStamp()
        {
            if (firstTimestampConstraint.Ticks != 0)
                return firstTimestampConstraint;
            return new DateTime(((Timestamp)timestamps[0]).GetTimestamp());
        }

        public DateTime LastTimeStamp()
        {
            if (lastTimestampConstraint.Ticks != 0)
                return lastTimestampConstraint;
            return new DateTime(((Timestamp)timestamps[timestamps.Count - 1]).GetTimestamp());
        }

        public long[] GetDistribution(int steps, string type)
        {
            int index = distrubutionCacheKey.IndexOf(type + steps);
            if (index > -1)
                return (long[])((long[])distributionCacheItem[index]).Clone();

            long firstTimeStamp = FirstTimeStamp().Ticks;
            long lastTimeStamp = LastTimeStamp().Ticks;
            decimal spanTimeStamp = lastTimeStamp - firstTimeStamp;

            long[] distribution = new long[steps];

            IEnumerator timestampEnumerator = timestamps.GetEnumerator();
            while(true)
            {
                if (!timestampEnumerator.MoveNext())
                    break;
                Timestamp ts = (Timestamp)timestampEnumerator.Current;
                if (ts.GetTimestamp() >= firstTimeStamp)
                    break;
            }

            for (int pos = 0; pos < steps; pos++)
            {
                long count = 0;
                long end = (long)((decimal)firstTimeStamp + (decimal)spanTimeStamp * (decimal)(pos + 1) / (decimal)steps);
                while (true)
                {
                    Timestamp ts = (Timestamp)timestampEnumerator.Current;
                    if (ts.GetTimestamp() > end)
                        break;
                    if (!timestampEnumerator.MoveNext())
                        break;
                    if (!ts.GetTimestampOrigin().Equals(type))
                        continue;

                    count++;
                }
                distribution[pos] = count;
            }

            distrubutionCacheKey.Add(type + steps);
            distributionCacheItem.Add(distribution);

            return (long[])distribution.Clone();
        }

        public string[] ActiveHandlers()
        {
            List<string> handlersInUse = new List<string>();
            IEnumerator timestampEnumerator = timestamps.GetEnumerator();
            while (true)
            {
                if (!timestampEnumerator.MoveNext())
                    break;
                Timestamp ts = (Timestamp)timestampEnumerator.Current;
                if (handlersInUse.IndexOf(ts.GetTimestampOrigin()) < 0)
                    handlersInUse.Add(ts.GetTimestampOrigin());

            }
            return handlersInUse.ToArray();
        }

        public XmlDatabase(string fileName)
        {
            XmlTextReader xml = new XmlTextReader(fileName);

            int count = 0;

            while (!xml.EOF)
            {

                while (!xml.EOF && (xml.NodeType != XmlNodeType.Element || xml.Name != "Evidence"))
                {
                    xml.Read();
                }

                string title = xml.GetAttribute("title");
                string type = xml.GetAttribute("type");
                string id = xml.GetAttribute("id");
                string parent = xml.GetAttribute("parent");

                EvidenceObject parentObject;
                if (parent == "" || parent == null)
                    parentObject = null;
                else
                    parentObject = GetEvidenceObject(Int32.Parse(parent));


                EvidenceObject evidenceObject = new EvidenceObject(title, type, id, parentObject);
                evidences.Add(evidenceObject);

                //Read evidence start tag here
                xml.Read();

                while (!xml.EOF && xml.NodeType != XmlNodeType.EndElement)
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        if (xml.Name == "Timestamp")
                        {
                            string timestampType = xml.GetAttribute("type");
                            string timestampValue = xml.GetAttribute("value");
                            string timestampOrigin = xml.GetAttribute("origin");
                            string timestampTitle = xml.GetAttribute("title");

                            evidenceObject.addProperty(timestampType, timestampValue);

                            DateTime datetime = DateTime.Parse(timestampValue);


                            if (datetime.Year > 1980 && datetime.Year < 2020 && timestampOrigin != null)
                                timestamps.Add(new Timestamp(timestampType, datetime.Ticks, timestampOrigin, evidenceObject));
                            count++;
                        }
                        else if (xml.Name == "Data")
                        {
                            string name = xml.GetAttribute("name");
                            string value = xml.GetAttribute("value");

                            evidenceObject.addProperty(name, value);
                        }
                        else if (xml.Name == "Chunk")
                        {
                            string from = xml.GetAttribute("from");
                            string to = xml.GetAttribute("to");

                            evidenceObject.addChunk(Int64.Parse(from), Int64.Parse(to));
                        }

                    }
                    xml.Read();
                }
            }
            timestamps.Sort();
        }
    }
}
