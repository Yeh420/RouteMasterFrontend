8/14(8/18前完成項目)   
1.[OK]普通登入功能
    [OK]登入的錯誤訊息調整
    [OK]登入功能錯誤不要刷新改成ajax       //1.5hr
    [OK]last step 檢查程式碼邏輯                     

2.[]Google登入功能
    []Google登入的RM Logo
    []google登入的認證方式要包含信箱 
    []last step 檢查程式碼邏輯
    [OK]成功登入並抓到登入者資訊
    
3.[x]index
    [x]顯示名稱
    [x]顯示項目調整
    [x]版面
    [x]last step 檢查程式碼邏輯l
    []abc 類別要改掉

4.[]歷史訂單
    []使用ajax呼叫到 
    []last step 檢查程式碼邏輯

5.[]大頭貼項
    []更改大頭貼
    []加入其他人要做的navbar
    []last step 檢查程式碼邏輯

6.[]發送信件/更換密碼
    []last step 檢查程式碼邏輯

7.[]修改會員資料
    []last step 檢查程式碼邏輯

8.[]鎖定帳號
    []last step 檢查程式碼邏輯

9.[]登出
    []登出後刪除google的資料
    []last step 檢查程式碼邏輯

10.[]演示會員資料
    []加入圖片
    []last step 檢查程式碼邏輯

11.[]註冊會員
   []
   []last step 檢查程式碼邏輯

12.[]登入時間計算


實作步驟
一.登入登出
    1.用已經設定好的帳號示範密碼錯誤
    2.確認登入
    3.登出一次

    4.正常註冊會員
    5.註冊一次
    6.立即再登入一次

    7.Google註冊登入
    8.註冊並登入

二.編輯會員資料
    1.更改電話號碼
    2.更改密碼
    3.查看大頭貼/更改大頭貼
    4.查詢歷史訂單

    
-------------------------------------------------------------------

可優化部分
1.Edit

@*<div>
    <a asp-action="MemOrder" asp-route-id="@Model?.Id">歷史訂單</a> |
    <a asp-action="Edit" asp-route-id="@Model?.Id">Edit</a> |
    <a asp-action="HistoryOrder" asp-route-id="@Model?.Id">歷史訂單</a> |
    <button class="btn btn-warning" onclick="getOrder()">查詢歷史訂單</button>
</div>*@



會員中心的清單列:使用div show and hide
就效能來說，一個是顯示的資料量不多
再來是就算顯示的資料量多龐大，也可以自己設定資料量
用ajax來顯示show more


model用一個，其他的可以用ajax呼叫


vm轉成ajax的欄位驗證問題
目前解決方法:
1.使用components，去渲染部分畫面
2.使用wwwroot/js/jquery-validation 去寫驗證欄位


<Form>裡面的submit或是button都會造成有sumbit的效果，以及submit都是會造成刷新頁面


如果要改成ajax 必須preventdafault


partial view  & viewcomponent的差別
p 為靜態去做變動， c為動態的去做變動，因此p的前端頁面如果有做判斷會比較亂，但如果用c去做的話，可以把邏輯都寫到
ivoke裡面

搞清楚renderaction & renderpartial的差別，這是core才改成的建議方式

在mvc裡面的ajax有許多的種類變化，再加上動詞就更多了，之後可以試試看get跟post的差別，確定傳東西是否可以不用類別的方式

partial view


使用ajax又想要驗證，使用javascript會比較好，但原始密碼要去後端抓，像是其他欄位的compare的話 用javascript來做比對
同理，其他欄位也是

前後端的restful? 

***layout=null***錯誤
在login的畫面，要用layout = null 發生了 http4xx的錯誤，但結果是 jquery的引用錯誤，導致後續一連串錯誤，也因此有些錯誤沒被呼叫到，反而變對的
F12顯示的錯誤不能忽略，當layout改成null後，要把html的等等屬性標籤等加回來
