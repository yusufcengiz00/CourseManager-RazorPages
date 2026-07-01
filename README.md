# CourseManager - Kurs ve Sertifika Yönetim Sistemi

CourseManager, ASP.NET Core Razor Pages mimarisi kullanılarak geliştirilmiş modern ve kullanıcı dostu bir Kurs ve Sertifika Yönetim portalıdır. Proje, hem genel kullanıcılar için kurs tanıtımlarının yer aldığı bir **Kullanıcı Portalı** hem de yöneticiler için öğrenci, kurs ve sertifika verilerinin yönetildiği gelişmiş bir **Yönetim Paneli** içerir.

## Özellikler

*   **Kullanıcı Portalı (Salt Okunur):**
    *   Akademinin misyonu ve vizyonunu sunan modern, etkileşimli karşılama (Hero) alanı.
    *   Veritabanından dinamik olarak çekilen ve listelenen aktif kurs tanıtımları (Kurs Adı, Eğitmen, Süre, Başlangıç Tarihi ve Fiyat).
    *   Sağ üstte konumlanan ve tek tıkla yönetim paneline geçiş sağlayan **Yönetici Girişi** butonu.
*   **Yönetici Paneli (CRUD):**
    *   **Dashboard:** Sistemdeki toplam öğrenci, aktif kurs ve kayıtlı sertifika sayılarını gösteren modern metrik kartları.
    *   **Öğrenci Yönetimi:** Yeni öğrenci ekleme, listeleme, düzenleme, silme ve arama.
    *   **Kurs Yönetimi:** Kurs ekleme, listeleme, düzenleme ve silme.
    *   **Sertifika Yönetimi:** Sertifika numarası ve tarihi ile sertifika kaydı yönetimi.
    *   **Analiz & Raporlama:** Kursların toplam finansal değeri, ortalama kurs süresi, en kapsamlı kurs bilgisi ve son 30 gün içinde kayıt olan öğrenci istatistikleri. Bu verileri PDF ve Excel formatında indirebilirsiniz.

## Teknoloji Yığını

*   **Backend:** .NET 10.0, ASP.NET Core Razor Pages
*   **Veritabanı:** SQL Server (LocalDB \ MSSQLLocalDB) + ADO.NET (SqlConnection, SqlCommand, SqlDataReader)
*   **Frontend:** HTML5, CSS3 (Custom Glassmorphism Design System), Bootstrap 5, Font Awesome 6, Google Fonts (Inter)

## Veritabanı Kurulumu

Proje, yerel SQL Server LocalDB örneği üzerinde `CourseManagerDB` veritabanını kullanmaktadır. Veritabanını ve tabloları oluşturmak için aşağıdaki SQL kodunu kullanabilirsiniz:

```sql
-- 1. Veritabanını oluşturma
CREATE DATABASE CourseManagerDB;
GO

USE CourseManagerDB;
GO

-- 2. Kurslar Tablosu
CREATE TABLE Courses (
    CoursesID INT IDENTITY(1,1) PRIMARY KEY,
    CourseName VARCHAR(100) NOT NULL,
    Instructor VARCHAR(100) NOT NULL,
    Duration INT,
    Price DECIMAL(18,2),
    StartDate DATE
);
GO

-- 3. Öğrenciler Tablosu
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    RegistrationDate DATE
);
GO

-- 4. Sertifikalar Tablosu
CREATE TABLE Certificates (
    CertificateID INT IDENTITY(1,1) PRIMARY KEY,
    IssueDate DATE,
    CertificateNo VARCHAR(50) NOT NULL
);
GO
```

## Çalıştırma Talimatları

1.  Bilgisayarınızda .NET SDK (10.0 veya üzeri) yüklü olduğundan emin olun.
2.  SQL Server LocalDB'nin çalıştığından emin olun.
3.  Proje ana dizinine gidin:
    ```bash
    cd CourseManager&RazorPages/CourseManager&RazorPages
    ```
4.  Projeyi derleyin ve çalıştırın:
    ```bash
    dotnet run
    ```
5.  Tarayıcınızda açılan adresten (varsayılan: `https://localhost:5001` veya `http://localhost:5000`) kullanıcı portalına erişebilirsiniz.


---

## 📸 Uygulama Görselleri

### Arayüz

<p align="center">
<img width="1527" height="902" alt="image" src="https://github.com/user-attachments/assets/921dfc8c-e933-45fa-ab66-709c69d66f1e" />
</p>

---

### Admin Panel Giriş Ekranı

<p align="center">
<img width="1563" height="903" alt="image" src="https://github.com/user-attachments/assets/5365f198-bdf5-4334-b9c9-0044be7487c4" />
</p>

---

### Öğrenci Yönetim Ekranı

<p align="center">
<img width="1562" height="911" alt="image" src="https://github.com/user-attachments/assets/717fc400-73f5-4be6-abec-0c5137559b06" />
</p>

---

### Kurs Yönetim Ekranı

<p align="center">
<img width="1570" height="910" alt="image" src="https://github.com/user-attachments/assets/a950299c-a4e5-448e-94cf-81193256ea9f" />
</p>

---

### Sertifika Yönetim Ekranı

<p align="center">
<img width="1571" height="908" alt="image" src="https://github.com/user-attachments/assets/bf7484c4-3651-4cd3-80ee-3bc51f9569e6" />
</p>


---

### Sistem Raporu Ekranı

<p align="center">
<img width="1632" height="781" alt="image" src="https://github.com/user-attachments/assets/f7dce430-1f4b-411d-907c-0823edc541fa" />

</p>

