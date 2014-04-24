using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Windows.Forms;

namespace MongoTrialApp
{
   public class MongoClass
   {
      // http://docs.mongodb.org/ecosystem/tutorial/getting-started-with-csharp-driver/#getting-started-with-csharp-driver

      public static void RunEntityExample()
      {

         // client
         var connectionString = "mongodb://localhost";
         var client = new MongoClient(connectionString);


         // server
         var server = client.GetServer();


         // db
         string databaseName = "test";
         var database = server.GetDatabase(databaseName);


         // collection object
         string collectionName = "entities_driver";
         var collection = database.GetCollection<Entity>(collectionName);


         // Insert document
         var entity = new Entity { Name = "Tom" };
         collection.Insert(entity);
         var id = entity.Id;


         // Find document
         var query = Query<Entity>.EQ(e => e.Id, id);
         entity = collection.FindOne(query);


         // Save changes - from Tom to Dick
         entity.Name = "Dick";
         collection.Save(entity);


         // Update doc - same as update
         // difference: save sends the entire doc; update sends changes
         //var query2 = Query<Entity>.EQ(e => e.Id, id);
         var update = Update<Entity>.Set(e => e.Name, "Harry");
         collection.Update(query, update);


         // Remove doc
         //var query3 = Query<Entity>.EQ(e => e.Id, id);
         collection.Remove(query);
      }

      // http://docs.mongodb.org/ecosystem/tutorial/use-csharp-driver/#csharp-driver-tutorial

      public static void RunBsonDocumentExample()
      {
         // server
         MongoServerSettings serverSettings = new MongoServerSettings();
         string host = "localhost";
         int port = 27017;
         serverSettings.Server = new MongoServerAddress(host, port);
         MongoServer server = new MongoServer(serverSettings);



         // db
         string databaseName = "test";
         var database = server.GetDatabase(databaseName);


         // insert
         string collectionName = "users_driver";
         var collection = database.GetCollection(collectionName);

         // recommended way to create and initialize
         var document = new BsonDocument{
            {"Name", "Peter"},
            {"ModifiedDate", DateTime.Now}
         };
         collection.Insert(document);

         // compiler's way:
         document = new BsonDocument();
         document.Add("Name", "Mary");
         document.Add("ModifiedDate", DateTime.Now);
         collection.Insert(document);

         // nested:
         BsonDocument nested = new BsonDocument {
            { "Name", "John" },
            { "ModifiedDate", DateTime.Now},
            { "Address", new BsonDocument {
               { "Street", "123 Main St." },
               { "City", "Vancouver" },
               { "Province", "BC" },
               { "PostalCode", "V1V1V1"}
            }}
         };
         collection.Insert(nested);

         BsonDocument[] batch = {
            new BsonDocument {
               { "Name", "Ann" },
               { "ModifiedDate", DateTime.Now}
            },
            new BsonDocument {
               { "Name", "Betty" },
               { "ModifiedDate", DateTime.Now},
               { "Seen", false }
            }};
         collection.InsertBatch(batch);



         // reading 
         //var collection_bson = database.GetCollection(collectionName).FindAll().SetSortOrder(SortBy.Ascending("Name"));
         //foreach (var user in collection_bson)
         //{
         //   var getname = user["Name"].AsString;
         //}


         // Find document
         var query = Query.EQ("Name", "Peter");
         var found = collection.Find(query);
         //if (found.Any())
         //   MessageBox.Show("Found: Peter; count: {0}", found.Count().ToString());
         string msg = string.Empty;
         foreach (var item in found)
         {            
            msg += item["Name"].AsString + "\n";
         }
         //MessageBox.Show(msg);

         // Save changes - from Tom to Dick


         // Update doc - same as update
         // difference: save sends the entire doc; update sends changes


         // Remove doc


         // client
         string connectionString = "mongodb://jason_laptop3";
         var client = new MongoClient(connectionString);

         server = client.GetServer();
         databaseName = "test";
         database = server.GetDatabase(databaseName);
         collectionName = "users_driver";
         collection = database.GetCollection(collectionName);

         // test?
         query = Query.EQ("Name", "Peter");
         var found2 = collection.FindOne(query);

      }
   }

   public class Entity
   {
      public ObjectId Id { get; set; }
      public string Name { get; set; }
   }
}
