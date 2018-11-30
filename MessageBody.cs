using Newtonsoft.Json;

namespace EventHubProcessingNet
{
    public class MessageBody
    {
        
        public Machine Machine { get; set; }

        
        public Ambient Ambient { get; set; }

        
        public string TimeCreated { get; set; }
    }
    
    public class Ambient

    {

        

        public double Temperature { get; set; }

        

        public int Humidity { get; set; }

    }

    

    public class Machine

    {

        

        public double Temperature { get; set; }

        

        public double Pressure { get; set; }

    }



}