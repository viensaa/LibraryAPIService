namespace TransaksiService.DomainObject.Mahasiswa
{
    public class DataMahasiswa
    {
        public int Id { get; set; }
        public string NIM { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
    }

    public class listMahasiswa : BaseResponse
    {
        public IEnumerable<DataMahasiswa> datas { get; set; }
    }
}
