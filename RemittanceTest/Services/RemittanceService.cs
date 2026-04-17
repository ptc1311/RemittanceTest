using RemittanceTest.Models;

namespace RemittanceTest.Services
{
    public class RemittanceService : IRemittanceService
    {
        // 模擬資料庫 (靜態變數確保跨 Request 資料一致)
        private static readonly List<Remittance> _db = new()
        {
            new Remittance { Id = 1, AccountName = "測試企業A", Amount = 50000, Status = 0 },
            new Remittance { Id = 2, AccountName = "測試企業B", Amount = 12000, Status = 1 }, // 不可取消
            new Remittance { Id = 3, AccountName = "測試企業C", Amount = 30000, Status = 0 }
        };

        // 提示：如何確保多執行緒下的資料安全？
        private static readonly object _lockObj = new object();

        public (bool IsSuccess, string Message) CancelRemittance(int id)
        {
            // TODO: 請在此處實作「取消」的商業邏輯與防併發檢核
            // 1. validate id exists
            // 2. validate status == 0
            // 3. handle concurrency (lock)
    
            var item = _db.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return (false, "NOT_FOUND");

            // ❗ 核心規則：只有 Status = 0 才能取消
            if (item.Status != 0)
                return (false, "INVALID_STATUS");

            item.Status = 9;

            return (true, "SUCCESS");
            throw new NotImplementedException();
        }
    }
}
