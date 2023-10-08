using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    [BsonIgnoreExtraElements]
    public class Application
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Dob { get; set; }
        public List<string>? PhoneList { get; set; }
        public List<string>? EmailList { get; set; }
        public List<Education>? EduList { get; set; }
        public List<Experience>? ExpList { get; set; }
        public List<Project>? ProList { get; set; }
        public List<string>? Skill { get; set; }

    }
}