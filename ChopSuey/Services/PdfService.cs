using ChopSuey.Areas.Identity.Data;
using ChopSuey.Contracts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Path = System.IO.Path;

namespace ChopSuey.Services
{
    public class PdfService : IPdfService
    {
        private readonly UserManager<ApplicationUser> _userManager;
       
        public PdfService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<byte[]> CreatePdfAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            string rolesString = string.Join(", ", roles);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string outputPdfPath = $"UserInfo_{DateTime.Now:yyyyMMddHHmm}.pdf";
            MemoryStream ms = new MemoryStream();
            using (Document document = new Document(PageSize.A4))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                string fontPathIranSanse = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/iransansxvf.ttf");
                string fontPathShabnam = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/shabnam-bold-fd.ttf");
                BaseFont baseFontIransanse = BaseFont.CreateFont(fontPathIranSanse, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont baseFontshabnam = BaseFont.CreateFont(fontPathShabnam, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontIranSanse = new Font(baseFontIransanse, 10, Font.BOLD, BaseColor.BLACK);
                Font fontShabnam = new Font(baseFontshabnam, 10, Font.BOLD, BaseColor.WHITE);

                PdfPTable title = new PdfPTable(1);

                title.WidthPercentage = 100;

                title.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell celltitel1 = new PdfPCell(new Phrase($"تاریخ : {DateTime.Now:yyyy-MM-dd} ", fontIranSanse));
                celltitel1.HorizontalAlignment = Element.ALIGN_LEFT;
                celltitel1.Padding = 5;
                celltitel1.BorderColor = BaseColor.WHITE;
                title.AddCell(celltitel1);
                PdfPCell celltitel2 = new PdfPCell(new Phrase("عنوان : اطلاعات کاربری", fontIranSanse));
                celltitel2.HorizontalAlignment = Element.ALIGN_LEFT;
                celltitel2.Padding = 5;
                celltitel2.PaddingBottom = 15;
                celltitel2.BorderColor = BaseColor.WHITE;
                title.AddCell(celltitel2);

                PdfPTable table = new PdfPTable(7);

                float[] columnWidths = new float[] { 2f, 2f, 2f, 1f, 3f, 2f, 1f };
                table.SetWidths(columnWidths);
                table.WidthPercentage = 100;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                PdfPCell cell = new PdfPCell(new Phrase("نام", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("نام خانوادگی", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ایمیل", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("سن", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("تلفن", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("شهر", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("نقش", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.firstName, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.lastName, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.Email, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.age.ToString(), fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.PhoneNumber, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(user.city, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(rolesString, fontIranSanse));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                table.AddCell(cell);

                document.Add(title);
                document.Add(table);
                document.Close();
            }

            return ms.ToArray();
        }

        public async Task<byte[]> GenerateUserListPdfAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string outputPdfPath = $"UserInfo_{DateTime.Now:yyyyMMddHHmm}.pdf";
            MemoryStream ms = new MemoryStream();
            using (Document document = new Document(PageSize.A4))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                string fontPathIranSanse = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/iransansxvf.ttf");
                string fontPathShabnam = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/shabnam-bold-fd.ttf");
                BaseFont baseFontIransanse = BaseFont.CreateFont(fontPathIranSanse, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont baseFontshabnam = BaseFont.CreateFont(fontPathShabnam, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontIranSanse = new Font(baseFontIransanse, 10, Font.BOLD, BaseColor.BLACK);
                Font fontShabnam = new Font(baseFontshabnam, 10, Font.BOLD, BaseColor.WHITE);

                PdfPTable title = new PdfPTable(1);

                title.WidthPercentage = 100; 

                title.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell celltitel1 = new PdfPCell(new Phrase($"تاریخ : {DateTime.Now:yyyy-MM-dd} ", fontIranSanse));
                celltitel1.HorizontalAlignment = Element.ALIGN_LEFT;
                celltitel1.Padding = 5;
                celltitel1.BorderColor = BaseColor.WHITE;
                title.AddCell(celltitel1);
                PdfPCell celltitel2 = new PdfPCell(new Phrase("عنوان : اطلاعات کاربری", fontIranSanse));
                celltitel2.HorizontalAlignment = Element.ALIGN_LEFT;
                celltitel2.Padding = 5;
                celltitel2.PaddingBottom = 15;
                celltitel2.BorderColor = BaseColor.WHITE;
                title.AddCell(celltitel2);

                PdfPTable table = new PdfPTable(7);

                float[] columnWidths = new float[] { 2.5f, 2f, 3f, 1.5f, 3.5f, 2.5f, 2f };
                table.SetWidths(columnWidths);
                table.WidthPercentage = 100;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                PdfPCell cell = new PdfPCell(new Phrase("نام", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("نام خانوادگی", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("ایمیل", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("سن", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("تلفن", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("شهر", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("نقش", fontShabnam));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 10;

                cell.BackgroundColor = BaseColor.DARK_GRAY;
                table.AddCell(cell);

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    string rolesString = string.Join(", ", roles);
                    cell = new PdfPCell(new Phrase(user.firstName, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(user.lastName, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(user.Email, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(user.age.ToString(), fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(user.PhoneNumber, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(user.city, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(rolesString, fontIranSanse));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 10;
                    table.AddCell(cell);

                }
                document.Add(title);
                document.Add(table);
                document.Close();
            }
            return ms.ToArray();
        }
    }
}
