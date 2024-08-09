namespace TransaksiService.DomainObject.Mahasiswa
{
    public class CreateMahasiswa
    {
        public string NIM { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
        public int RowStatus { get; private set; } = 1;
    }
}
