using System;
using System.Collections.Generic;
using System.Linq;

abstract class PhuongTien
{
    private string _maPT;
    private string _tenHang;
    private int _namSanXuat;
    private decimal _giaGoc;

    public string MaPT
    {
        get { return _maPT; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                _maPT = "PT000";
            else
                _maPT = value;
        }
    }

    public string TenHang
    {
        get { return _tenHang; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ten hang khong duoc de trong!");
            _tenHang = value;
        }
    }

    public int NamSanXuat
    {
        get { return _namSanXuat; }
        set
        {
            if (value < 1900 || value > DateTime.Now.Year)
                throw new ArgumentException("Nam san xuat khong hop le!");
            _namSanXuat = value;
        }
    }

    public decimal GiaGoc
    {
        get { return _giaGoc; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Gia goc phai lon hon 0!");
            _giaGoc = value;
        }
    }

    public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
    {
        MaPT = maPT;
        TenHang = tenHang;
        NamSanXuat = namSanXuat;
        GiaGoc = giaGoc;
    }

    public abstract decimal TinhGiaLanBanh();

    public virtual string GetInfo()
    {
        return $"Ma: {MaPT}, Hang: {TenHang}, Nam SX: {NamSanXuat}, Gia goc: {GiaGoc:N0}";
    }
}

class OTo : PhuongTien
{
    public int SoChoNgoi { get; set; }
    public double DungTichDongCo { get; set; }

    public OTo(
        string maPT,
        string tenHang,
        int namSanXuat,
        decimal giaGoc,
        int soChoNgoi,
        double dungTichDongCo)
        : base(maPT, tenHang, namSanXuat, giaGoc)
    {
        if (soChoNgoi <= 0)
            throw new ArgumentException("So cho ngoi phai lon hon 0!");

        if (dungTichDongCo <= 0)
            throw new ArgumentException("Dung tich dong co phai lon hon 0!");

        SoChoNgoi = soChoNgoi;
        DungTichDongCo = dungTichDongCo;
    }

    public override decimal TinhGiaLanBanh()
    {
        if (SoChoNgoi <= 9)
            return GiaGoc + GiaGoc * 0.12m + GiaGoc * 0.30m;

        return GiaGoc + GiaGoc * 0.10m;
    }

    public override string GetInfo()
    {
        return base.GetInfo() +
               $", So cho: {SoChoNgoi}, Dong co: {DungTichDongCo}L";
    }
}

class XeMay : PhuongTien
{
    public int DungTichXylanh { get; set; }

    public XeMay(
        string maPT,
        string tenHang,
        int namSanXuat,
        decimal giaGoc,
        int dungTichXylanh)
        : base(maPT, tenHang, namSanXuat, giaGoc)
    {
        if (dungTichXylanh <= 0)
            throw new ArgumentException("Dung tich xylanh phai lon hon 0!");

        DungTichXylanh = dungTichXylanh;
    }

    public override decimal TinhGiaLanBanh()
    {
        if (DungTichXylanh < 175)
            return GiaGoc + GiaGoc * 0.02m;

        return GiaGoc + GiaGoc * 0.05m;
    }

    public override string GetInfo()
    {
        return base.GetInfo() +
               $", Xylanh: {DungTichXylanh}cc";
    }
}

class QuanLyPhuongTien
{
    private List<PhuongTien> danhSach = new List<PhuongTien>();

    public void AddPhuongTien(PhuongTien pt)
    {
        danhSach.Add(pt);
    }

    public void DisplayAll()
    {
        foreach (PhuongTien pt in danhSach)
        {
            Console.WriteLine(pt.GetInfo());
            Console.WriteLine(
                $"Gia lan banh: {pt.TinhGiaLanBanh():N0} VNĐ");
            Console.WriteLine();
        }
    }

    public PhuongTien FindMaxGiaLanBanh()
    {
        return danhSach
            .OrderByDescending(pt => pt.TinhGiaLanBanh())
            .FirstOrDefault();
    }

    public List<PhuongTien> SearchByName(string keyword)
    {
        return danhSach
            .Where(pt => pt.TenHang.Contains(
                keyword,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

class Program
{
    static void Main()
    {
        QuanLyPhuongTien ql = new QuanLyPhuongTien();

        OTo oto = new OTo(
            "OT01",
            "Toyota",
            2024,
            1000000000m,
            5,
            2.0);

        XeMay xemay = new XeMay(
            "XM01",
            "Honda",
            2023,
            50000000m,
            150);

        ql.AddPhuongTien(oto);
        ql.AddPhuongTien(xemay);

        Console.WriteLine("===== DANH SACH PHUONG TIEN =====");
        ql.DisplayAll();

        Console.WriteLine("===== GIA LAN BANH CAO NHAT =====");

        PhuongTien max = ql.FindMaxGiaLanBanh();

        if (max != null)
        {
            Console.WriteLine(max.GetInfo());
            Console.WriteLine(
                $"Gia lan banh: {max.TinhGiaLanBanh():N0} VNĐ");
        }

        Console.WriteLine();
        Console.WriteLine("===== TIM KIEM HONDA =====");

        List<PhuongTien> ketQua = ql.SearchByName("Honda");

        foreach (PhuongTien pt in ketQua)
        {
            Console.WriteLine(pt.GetInfo());
        }
    }
}
