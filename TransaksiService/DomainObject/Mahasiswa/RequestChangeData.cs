namespace TransaksiService.DomainObject.Mahasiswa
{
    public class RequestChangeData
    {
        //public int Id { get; set; }
        public string NIM { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
        public int RowStatus { get; set; }
    }
}
