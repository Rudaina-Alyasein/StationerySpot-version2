namespace stationerySpot.Models
{
    public class ChatMessageViewModel
    {
        public int SenderId { get; set; }       // اليوزر أو المكتبة
        public int ReceiverId { get; set; }     // الطرف الآخر
        public string Message { get; set; }     // محتوى الرسالة
        public DateTime Timestamp { get; set; }

    }

}
