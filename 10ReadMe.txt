2023/07/31
[]





















***先實作出來，每一個改的地方記錄下來，節省複製到總檔案的時間
**一個一個確認無bug的做出來，除bug大於21分鐘就詢問別人
**前端是使用VUE的javascript方法，但一定要學會restful方法!!

功能列表
1.Google登入						[點擊goolge登入][][登入後跳轉到主頁]
	1.Google登入的頁面
2.普通登入							[點擊會員登入][登入後跳轉到主頁][且只能看到自己的東西]
	2.普通登入的頁面
3.google註冊(註冊後才可以綁定帳號)  [登入後跳轉到主頁]
4.普通註冊							[登入後跳轉到主頁]
5.編輯會員資料						[編輯完跳轉到該會員頁]
6.查詢歷 史訂單						
7.刪除會員							[刪除後自動登出][此設計為停權，並不真的刪除]
8.登入三次即鎖定
9.Line登入
10.密碼強度限制

----------以下實作

[]普通登入(使用Account，密碼)(登入後)
	1.先做一個action
	2.寫裡面語法
	  最後是新增檢視
	3.完成到登入後，但因為datatime的格式問題，要把每個會員都加上生日


[]google登入&包含註冊 (使用google信箱，密碼由google抓取)

[√]註冊
	1.創建ViewModel (檢驗)
	2.IActionResult MemberRegister
		*RegisterMember
	3.新增檢視
[]Authentication
	1.startup.cs加上app.UseAuthentication();
	2.


[]大頭貼顯示位置，只要在右上角的小小資訊欄顯示即可，在index頁並不用顯示，

!!!gitHub要修正的地方
1.註冊的生日
	*Vm
	*註冊.cshtml


----------簡歷

一.會員與後台管理人員
    (一).後台
        1.會員資料管理(CRUD)
        2.會員停權管理
		3.後臺管理人員權限管理(分權瀏覽及操作不同功能)
		4.密碼以Hash256進行雜湊處理

	(二).前台
		1.會員註冊(傳送信件開通會員資格)
		2.串接Google註冊登入
		3.會員登入(嘗試登入過多即鎖定)
		4.忘記密碼與重設密碼
		5.會員查詢歷史訂單
		
-------- extra function

**嘗試登入鎖定
//Result resultAttempt = LockAccountIfFailedAttemptsExceeded(vm.Account);
            //if (resultAttempt.IsSuccess)
            //{
            //    return resultAttempt; // 帳號已鎖定，返回錯誤結果
            //}


---------團體專題要修改的地方

*appsetting.json add salt