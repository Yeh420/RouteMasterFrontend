const dec = {
    data() {
        return {
            bfVM: [],
            indexVM: [],
            item: {},
            isReplyed: "已回復",
            ep: null,
            selected: 0,         
            thumbicon: [],
            hotelId: 0,



        }
    },
    //created: function () {
    //    let _this = this;
    //},
    //mounted: function () {
    //    let _this = this;
    //    _this.commentDisplay();
    //},
    methods: {
        commentBrief: function (id) {
            var request = {};
            let _this = this;
            if (id) {
                _this.hotelId = id;
            }
            request.Manner = _this.selected;
            request.HotelId = _this.hotelId;

            axios.post("https://localhost:7145/Comments_Accommodation/Index", request).then(response => {
                _this.bfVM = response.data;
                console.log(_this.bfVM);
            }

            ).catch(err => {
                alert(err);
            });

        },
        commentDisplay: function () {
            let _this = this;
            var request = {};
            //if (id) {
            //    _this.hotelId = id;
            //}
            request.Manner = _this.selected;
            request.HotelId = _this.hotelId;

            axios.post("https://localhost:7145/Comments_Accommodation/ImgSearch", request).then(response => {
                _this.indexVM = response.data;
                console.log(_this.indexVM);
                _this.thumbicon = _this.indexVM.map(function (vm) {
                    
                    return vm.thumbsUp ? '<i class="fa-solid fa-thumbs-up fa-lg"></i>' : '<i class="fa-regular fa-thumbs-up fa-lg"></i>';
                })
                

                for (let j = 0; j < _this.indexVM.length; j++) {
                    _this.item = _this.indexVM[j];
                }

            }).catch(err => {
                alert(err);
            });
        },       
        likeComment: async function (commentId) {
            let _this = this;
            var request = {};
            request.CommentId = commentId;
            request.IsLike = true;

            await axios.post("https://localhost:7145/Comments_Accommodation/DecideLike", request).then(response => {

                _this.commentDisplay();

            }).catch(err => {
                alert(err);
            });

        },
        getImgPath: function (photo) {
            return `@Url.Content("../MemberUploads/${photo}")`;
        },
        getProfile: function (photo) {
            return `@Url.Content("../SystemImages/${photo}")`;
        },
    },
    template: ` <div class="row g-2 mt-2 mb-3">
            <div v-for="(text, num) in bfVM" :key="num" class="col-md-4">
                <div class="card overflow-auto" style="max-height: 180px;">
                    <div class="card-body">
                        <div class="d-flex">
                            <h5 class="card-title me-auto">{{text.account}}</h5>
                            <p class="card-text">{{text.score}}<i class="fa fa-star fa-fw" style="color:#f90;"></i></p>
                        </div>
                        <div class="d-flex">
                            <p class="card-text me-auto">{{text.title}}</p>
                            <p class="card-text"><i class="fa-solid fa-thumbs-up"></i> {{text.totalThumbs}}</p>
                        </div>
                    </div>
                </div>
            </div>                
        </div>

        <div class="row mb-2">
            <div class="col-3">
                <select v-model="selected" id="commentOrder" @change="commentDisplay()">
                   <option value="0" selected>排序選擇</option>
                   <option value="1">最新留言</option>
                   <option value="2">星星評分高至低</option>
                   <option value="3">星星評分低至高</option>
                </select>
            </div>

            <div class="col-3 ms-auto text-end">
               <a asp-controller="Comments_Accommodation" asp-action="Create" asp-route-id="hotelId">撰寫評論</a>
            </div>
        </div>

       
        <div v-for="(item, index) in indexVM" :key="index" class="card mb-3">
            <div class="row overflow-auto" style="max-height:500px;">
                <div class="col-md-1 ms-auto">
                    <img :src="getProfile(item.profile)" style="height: 80px; width: 80px;" class="mx-auto" />  
                </div>
                <div class="col-md-11">
                    <div class="card-header bg-transparent border-success">
                        <ul class="list-unstyled">
                            <li class="fs-4">{{item.account}}</li>
                            <li>{{item.gender? "女性" : "男性"}} | {{item.address}}</li>
                        </ul>
                    </div>
                    <div class="card-body bg-light">
                        <div class="d-flex pb-0 mb-2">
                            <p>{{item.score}}<i class="fa fa-star fa-fw me-3" style="color:#f90;"></i></p>
                            <h4 class="card-title me-auto" v-if="item.title">{{item.title}}</h4>
                            <p class="text-muted">{{item.createDate}}</p>
                        </div>
                        <p class="card-text" v-if="item.pros">{{"優點:" + item.pros}}</p>
                        <p class="card-text" v-if="item.cons">{{"缺點:" + item.cons}}</p>
                        <ul v-if="item.imageList.length" class="d-flex list-unstyled mb-3">
                            <li v-for="(photo,num) in item.imageList" :key="num" class="me-3">
                                <img :src="getImgPath(photo)" style="height: 80px; width: 80px;" />
                            </li>
                        </ul>



                        <div class="d-flex mt-2">
                            <button type="button" v-html="thumbicon[index]" @click="likeComment(item.id)" class="btn btn-outline-dark me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="按讚">
                            </button>
                            <p class="card-text me-3"> {{item.totalThumbs}}</p>
                            <button v-if="item.status===isReplyed" type="button" class="btn btn-primary position-relative" data-bs-toggle="collapse"
                                    :data-bs-target='"#collapseExample"+index' @showReply(item.id)>
                                看回覆訊息
                            </button>
                        </div>

                        <template v-if="item.status===isReplyed">

                            <div class="collapse mt-3" :id='"collapseExample" + index'>
                                <div class="card card-body">
                                    <h5>來自{{item.hotelName}}的回覆:</h5>
                                    <p>{{item.replyMessage}}</p>
                                    <p class="card-text text-end"><small class="text-muted">{{item.replyDate}}</small></p>
                                </div>
                            </div>
                        </template>
                    </div>


                   
                </div>
               </div>                               
            </div>`
    
       


};
