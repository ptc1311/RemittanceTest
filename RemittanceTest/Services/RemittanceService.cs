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

            var threadId = Thread.CurrentThread.ManagedThreadId;
            var time = DateTime.Now.ToString("HH:mm:ss.fff");

            Console.WriteLine($"[{time}] Thread {threadId} -> Request START (Id={id})");

            lock (_lockObj) // 確保同一時間只有一個執行緒能操作 , TC+++:start
            {
                var enterTime = DateTime.Now.ToString("HH:mm:ss.fff");
                Console.WriteLine($"[{enterTime}] Thread {threadId} -> ENTER LOCK");

                var item = _db.FirstOrDefault(x => x.Id == id);

                if (item == null)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Thread {threadId} -> NOT FOUND");
                    return (false, "NOT_FOUND");
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Thread {threadId} -> Current Status = {item.Status}");

                // ❗ 核心規則：只有 Status = 0 才能取消
                if (item.Status != 0)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Thread {threadId} -> INVALID STATUS");
                    return (false, "INVALID_STATUS");
                }

                // only for test
                //Thread.Sleep(500);

                item.Status = 9;

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Thread {threadId} -> SUCCESS (Status changed to 9)");

                return (true, "SUCCESS");
            } //TC+++:end
            throw new NotImplementedException();
        }
    }
}
