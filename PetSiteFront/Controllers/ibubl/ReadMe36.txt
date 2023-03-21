前台 View Page  新增頁面
1. Opendata 抓JSon
-- 載入太慢
√-- 圖片破圖無法抓取
-- 開發環境Json 檔亦是
1.2 做Opendata data 篩選

PetSite API Controller
2. 會員兌換折價券
2.1 MemberCoupon Controller CRUD
√-- 兌換 -> 新增(Member) MemberCoupon
√-- 過期(Coupon) -> 修改 MemberCoupon Status (0未過期,1未兌換且過期)
√-- 無刪除功能
√--	查看已兌換 -> 該會員(id)所屬折價券清單
√-- 判斷memberId 跟CouponId 是否存在，再新增歸戶
☆ -- 使用(Cart) -> 修改 MemberCoupon UsedCoupon (0未使用,1已使用)
☆ -- 折價券過期，要將以歸戶的折價券狀態改成過期(或是先把過期的手動處理)

2.2 Coupon Controller 清單表
√ --查看所有未兌換Coupon -> Coupon List
	驗證：
	--使用期間√
	


3. 會員新增浪浪資料(走失、領養，拾獲)
√-- Controller CRUD 
√-- 抓取該Cookie會員(ID、Acount)發布的浪浪資料
√-- 將上傳圖片匯入到資料庫中
√-- 篩選資料(花面等待的時候太醜)
√-- 畫面
-->新增SQL 縣市/地區資料 DROPDOWNLIST 連動

√3.1 將Google Maps API 導入 (有時間再做)
--將3.貼文(資料)與(篩選)放置在地圖上
--系結地圖

-----------------------------------------------
前台畫面：
4. Adoptions
√--新增
√--刪除
√--會員刊登List
√--修改
√--所有刊登浪浪搜尋表
√--查詢
√--畫面List(id)details
 --驗證填寫欄位
√ --修改畫面與欄位優化
√ --新增畫面與欄位優化
 --畫面/欄位優化
 --篩選button 優化

4.1 地圖搜尋浪浪
-- 跟地圖繫結
-- 篩選

5. MemberCoupons
√--新增
√--不同狀態的會員優惠券列表
√ --畫面
 --修改(在Order使用改變false)

6. 串聯MemberAccount
√ -- 抓Cookie中的Account

7.Opendata Json 收容所浪浪
--畫面
--簡易列表
--篩選
--匯入速度
√--替代圖片
--json 資料無法在跟目錄下抓取
