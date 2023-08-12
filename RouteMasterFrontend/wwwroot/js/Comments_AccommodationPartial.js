const dec = {
    data() {
        return {
            indexVM: [],
            item: {},
            isReplyed: "已回復",
            ep: null,
            selected: 0,
            //hotelId: 1, //假設這是呈現AccomodationId=2的評論清單，這裡直接賦值=2
            thumbicon: [],
            hotelId:0

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
        commentDisplay: function (id) {
            let _this = this;
            var request = {};
            if (id) {
                _this.hotelId = id;
            }
            request.Manner = _this.selected;
            request.HotelId = _this.hotelId;

            axios.post("https://localhost:7145/Comments_Accommodation/ImgSearch", request).then(response => {
                _this.indexVM = response.data;
                console.log(_this.indexVM);
                _this.thumbicon = _this.indexVM.map(function (vm) {
                    //console.log(vm.thumbsUp);
                    return vm.thumbsUp ? '<i class="fa-solid fa-thumbs-up fa-lg"></i>' : '<i class="fa-regular fa-thumbs-up fa-lg"></i>';
                })
                //console.log(_this.thumbicon);

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
            return `../MemberUploads/${photo}`;
        }

    },
    template: `
        <div class="container">
        <div class="row mb-2" style="width:20%">
            <select v-model="selected" id="commentOrder" class="ms-3" @change="commentDisplay()">
                <option value="0" selected>排序選擇</option>
                <option value="1">最新留言</option>
                <option value="2">星星評分高至低</option>
                <option value="3">星星評分低至高</option>
            </select>
            <div class="ms-auto"><a href="https://localhost:7145/Comments_Accommodation/Create" class="link-dark">新增評論</a></div>
        </div>
         <div class="row g-4">
            <div v-for="(item, index) in indexVM" :key="index" class="col-4">
                <div class="card">
                    <template v-if="item.imageList.length>1">
                        <div :id='"carousel" + index' class="carousel carousel-dark slide" data-bs-ride="carousel">
                            <div class="carousel-inner mx-auto my-auto w-50">
                                <div :class="{ 'carousel-item': true, 'active': num === 0 }" v-for="(photo,num) in item.imageList" :key="num">
                                    <img v-bind:src="getImgPath(photo)" class="d-block card-img-top ">
                                </div>

                            </div>
                            <button class="carousel-control-prev" type="button" :data-bs-target='"#carousel"+ index' data-bs-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                <span class="visually-hidden">Previous</span>
                            </button>
                            <button class="carousel-control-next" type="button" :data-bs-target='"#carousel"+ index' data-bs-slide="next">
                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                <span class="visually-hidden">Next</span>
                            </button>
                        </div>
                    </template>
                    <template v-else>
                        <img src="../MemberUploads/RouteMaster.png" class="img-fluid card-img-top">
                    </template>
                    <hr/>
                    <div class="card-body">
                        <h5 class="card-title">標題: {{item.title}}</h5>
                        <p class="card-text">{{item.id}}</p>
                        <p class="card-text">{{item.hotelName}}</p>
                        <p class="card-text">優點: {{item.pros}}</p>
                        <p class="card-text">缺點: {{item.cons}}</p>
                        <p class="card-text">{{item.score}}</p>
                        <div class="d-flex">
                            <button type="button" v-html="thumbicon[index]" @click="likeComment(item.id)" class="btn btn-outline-dark me-3" data-bs-toggle="tooltip" data-bs-placement="top" title="按讚">
                            </button>
                            <p class="card-text me-auto"> {{item.totalThumbs}}</p>
                            <p><small class="text-muted">{{item.createDate}}</small></p>
                        </div>
                        <template v-if="item.status===isReplyed">
                            <hr />
                            <button type="button" class="btn btn-primary position-relative" data-bs-toggle="collapse"
                                    :data-bs-target='"#collapseExample"+index' @showReply(item.id)>
                                看回覆訊息
                            </button>
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
        </div>
    `

};
