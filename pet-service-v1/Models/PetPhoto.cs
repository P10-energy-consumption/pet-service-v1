namespace pet_service_v1.Models
{
    public class PetPhoto
    {
        public int PetID { get; set; }
        public string MetaData { get; set; }
        public FormFile File { get; set; }

        public PetPhoto(int petID, string metaData, FormFile file)
        {
            PetID = petID;
            MetaData = metaData;
            File = file;
        }
    }
}
