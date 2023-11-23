using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using DAL.Enums;

namespace CateringManagement.Repository
{
    public class ReportRepository : GenericRepository<Orders>
    {
        public async Task <List<ReportDTO>> GetDataReport(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var reports = new List<ReportDTO>();

                // Tạo danh sách các ngày cần thống kê
                var dates = Enumerable.Range(0, (dateEnd - dateStart).Days + 1)
                                     .Select(i => dateStart.AddDays(i))
                                     .ToList();

                //Duyệt qua từng ngày và tính toán doanh thu, tổng đơn hàng
                foreach (var date in dates)
                {
                    var revenue = db.Orders
                                .Where(od => od.CreatedAt.Date == date.Date && od.Status == OrderStatus.Done && od.IsDeleted != 1)
                    .Sum(od => od.TotalPrice);



                    var totalOrders = db.Orders
                                        .Where(o => o.CreatedAt.Date == date.Date && o.Status == OrderStatus.Done && o.IsDeleted != 1)
                                        .Count();

                    ReportDTO reportDto = new ReportDTO
                    {
                        DateValue = date,
                        Order = totalOrders,
                        TotalMoney = revenue
                    };

                    reports.Add(reportDto);
                }
                return reports;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
