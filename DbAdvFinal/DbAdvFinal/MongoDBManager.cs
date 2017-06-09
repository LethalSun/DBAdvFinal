using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace DbAdvFinal
{
    //컨테이너
    using BabyList = List<BabyNameInfo>;
    //아기 이름 정보 구조체
    class BabyNameInfo
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
        public int Count { get; set; }
    }

    class MongoDBManager
    {
        //디비 연결 정보
        private string m_ipAddress = null;
        private int m_portNumber = -1;
        private string m_userID = null;
        private string m_password = null;
        private string m_databaseName = null;

        //디비 정보
        private MongoClient m_mongoClient = null;
        private IMongoDatabase m_mongoDatabase = null;
        private FilterDefinitionBuilder<BsonDocument> m_builder = Builders<BsonDocument>.Filter;
        private FilterDefinition<BsonDocument> m_filter = "{}";
        private string m_CollectionName = null;

        //생성자, 디비서버에 연결, 데이터 베이스를 받아온다.
        public MongoDBManager(string pIP, int pPortNum, string pID, string pPW, string pDBName)
        {
            m_ipAddress = pIP;
            m_portNumber = pPortNum;
            m_userID = pID;
            m_password = pPW;
            m_databaseName = pDBName;

            var credential = MongoCredential.CreateCredential(m_databaseName, m_userID, m_password);

            var setting = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(m_ipAddress, m_portNumber)
            };

            m_mongoClient = new MongoClient(setting);
            m_mongoDatabase = m_mongoClient.GetDatabase(m_databaseName);
        }

        //람다나 제네릭하게 만들고 싶지만 시간이 없으니...
        //이 프로젝트에 의존적인 부분.
        //설정한 필터에 대한 데이터를 받아온다.
        //비동기로.
        public async Task<BabyList> GetResult()
        {
            BabyList returnList = new BabyList();
            //FilterDefinition<BsonDocument> DoNothingFilter = "{}";
            var collection = m_mongoDatabase.GetCollection<BsonDocument>("nationalBabyName");


            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(m_filter))
            {

                while (cursor.MoveNext())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        string name = null;

                        int id = 0;
                        if (document["Id"].IsNumeric == true)
                        {
                            id = document["Id"].AsInt32;
                        }

                        if (document["Name"].IsString == true)
                        {
                            name = document["Name"].AsString;
                        }

                        int year = 0;

                        if (document["Year"].IsNumeric == true)
                        {
                            year = document["Year"].AsInt32;
                        }
                        int count = 0;

                        if (document["Count"].IsNumeric == true)
                        {
                            count = document["Count"].AsInt32;
                        }

                        string gender = null;

                        if (document["Gender"].IsString == true)
                        {
                            gender = document["Gender"].AsString;
                        }

                        var info = new BabyNameInfo();

                        info.Name = name;
                        info.Year = year;
                        info.Gender = gender;
                        info.Count = count;

                        returnList.Add(info);
                    }
                }
            }

            return returnList;
            // 부탁받은거 호출해주기
        }

        public void SetCollection(string pCollectionName)
        {
            m_CollectionName = pCollectionName;
        }

        public void ClearFilter()
        {
            m_filter = "{}";
        }

        public void AddFilterStringEq(string pIndex, string pValue)
        {
            m_filter = m_filter & m_builder.Eq(pIndex, pValue);
        }

        public void AddFIlterStringRegex(string pIndex, string pValue)
        {
            m_filter = m_filter & m_builder.Regex(pIndex, pValue);
        }

        public void AddFilterEqInt(string pIndex, int pValue)
        {
            m_filter = m_filter & m_builder.Eq(pIndex, pValue);
        }

    }
}
