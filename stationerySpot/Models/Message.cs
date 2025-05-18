namespace stationerySpot.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int LibraryId { get; set; }

        public int UserId { get; set; }       // مفتاح خارجي للمستخدم
        public User User { get; set; }        // العلاقة مع المستخدم

        public string Sender { get; set; }    // مثلاً: "User" أو "Library"
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Library Library { get; set; }  // العلاقة مع المكتبة
    }
}