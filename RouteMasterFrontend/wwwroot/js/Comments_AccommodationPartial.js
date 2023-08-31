const dec = {
    data() {
        return { 
            indexVM: [],
            isReplyed: "已回復",
            selected: 0,         
            thumbicon: [],
            hotelId: 0,
            starpoint:0,
            title: "",
            pros: "",
            cons: "",
            showAlert: false,
            carouselList: [],
            userAccount:"",
            fileName: "",
            sliderMin: 1,
            sliderMax:10,
           



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
        commentDisplay: function (id, identity) {
            console.log(id)
            let _this = this;
            var request = {};
            if (id) {
                _this.hotelId = id;
            }

            if (identity) {
                _this.userAccount = identity;

            }



            request.Manner = _this.selected;
            request.HotelId = _this.hotelId;

            axios.post("https://localhost:7145/Comments_Accommodation/ImgSearch", request).then(response => {
                _this.indexVM = response.data;
                console.log(_this.indexVM);
                _this.thumbicon = _this.indexVM.map(function (vm) {
                    
                    return vm.thumbsUp ? '<i class="fa-solid fa-thumbs-up fa-lg"></i>' : '<i class="fa-regular fa-thumbs-up fa-lg"></i>';
                })
                

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
            return `/MemberUploads/${photo}`;
        },
        getProfile: function (photo) {
            return `/MemberUploads/${photo}`;
        },        
        sendForm: function () {
            let _this = this;
            if (_this.starpoint) {
                const formData = new FormData();
                formData.append('AccommodationId', _this.hotelId);
                formData.append('Score', _this.starpoint);
                formData.append('Title', _this.title);
                formData.append('Pros', _this.pros);
                formData.append('Cons', _this.cons);

                const file1 = document.querySelector("#file1");
                var totalFiles = file1.files.length;

                for (let i = 0; i < totalFiles; i++) {
                    formData.append('Files', file1.files[i]);
                }

                axios.post("https://localhost:7145/Comments_Accommodation/NewComment", formData).then(response => {

                    const result = response.data;
                    console.log(result);

                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: '評論新增成功',
                        showConfirmButton: false,
                        timer: 1000
                    })

                    _this.commentDisplay();

                    var closeBut = document.getElementById("shutDown");
                    var clickEvent = new Event("click", {
                        bubbles: false,
                        cancelable: true
                    });
                    closeBut.dispatchEvent(clickEvent);

                }).catch(err => {
                    alert(err);
                });
            }
            else {
                _this.showAlert = true;
                setTimeout(() => {
                    _this.showAlert = false; // 2秒后復原信息
                }, 2000);
            }
        },       
        clearWords: function () {
            let _this = this;  
            _this.starpoint =0,
            _this.title = "";
            _this.pros = "";
            _this.cons = "";
            _this.fileName = "";
            file1.value = "";
        },
        openImageModal: function (images, starIndex) {
            let _this = this;
            _this.carouselList = images;
            const imgModal = new bootstrap.Modal(document.getElementById('imgModal'));
            imgModal.show();

            const carouselImg = new bootstrap.Carousel(document.getElementById('carouselImg'), {
                interval: 1500,
                wrap: true,

            });

            var init = document.querySelector(`#carouselImg [data-bs-slide-to="${startIndex}"]`);

            carouselImg.from(init);
        },
        selectFiles: function () {
            let _this = this;
            var file1 = document.querySelector("#file1");
            var totalFiles = file1.files.length;
            _this.fileName = `${totalFiles}個檔案`
            console.log(_this.fileName);

        },
        scrollGrey: function(){
            const rangeInput = document.querySelector('#target');
            var percentage = 0;
            rangeInput.style.setProperty('--percentage', percentage);
        },
        colorShift: function () {
            let _this = this;
            const rangeInput = document.querySelector('#target');
            
                    
            const percentage = (_this.starpoint - _this.sliderMin) / (_this.sliderMax - _this.sliderMin);
            console.log(percentage);
            rangeInput.style.setProperty('--percentage', percentage);
           
        },

    },
    template: ` <div class="row mb-2">
            <div class="col-3">
                <select v-model="selected" id="commentOrder" @change="commentDisplay()">
                   <option value="0" selected>最新留言</option>
                   <option value="1">星星評分高至低</option>
                   <option value="2">星星評分低至高</option>
                </select>
            </div>

            <div class="col-3 ms-auto text-end">
                <button v-if="userAccount" type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#staticBackdrop" @click="scrollGrey">
                    撰寫評論
                </button>
            </div>
        </div>

       
        <div v-for="(item, index) in indexVM" :key="index" class="card mb-4 border-success">
            <div class="row overflow-auto" style="max-height:500px;">
                <div class="col-md-1 ms-auto">
                    <img :src="getProfile(item.profile)" style="height: 78px; width: 78px;" class="mx-auto" />  
                </div>
                <div class="col-md-11">
                    <div class="card-header bg-transparent border-success">
                        <ul class="list-unstyled">
                            <li class="fs-4">{{item.account}}</li>
                            <li>{{item.gender? "女性" : "男性"}} | {{item.address}}</li>
                        </ul>
                    </div>
                    <div class="card-body bg-light">
                        <div class="d-flex pb-0 mb-2 justify-content-center">
                            <p class="card-text bg-primary text-white d-flex justify-content-center align-items-center rounded-2 fs-6 me-2" style="width: 25px; height: 25px;">{{item.score}}</p>
                            <h4 class="card-title" v-if="item.title">{{item.title}}</h4>
                            <p class="ms-auto fs-6" style="color:lightseagreen;">{{item.createDate}}</p>
                        </div>
                        <p class="fs-5" v-if="item.pros">{{"優點: " + item.pros}}</p>
                        <p class="fs-5" v-if="item.cons">{{"缺點: " +  item.cons}}</p>
                        <ul v-if="item.imageList.length" class="d-flex list-unstyled mt-2 mb-3">
                            <li v-for="(photo,num) in item.imageList" :key="num" class="me-3">
                                <img :src="getImgPath(photo)" class="zoomIn" @click="openImageModal(item.imageList, num)" />
                            </li>
                        </ul>



                        <div class="d-flex mt-2 align-items-center">
                            <template v-if="userAccount">
                                <button type="button" v-html="thumbicon[index]" @click="likeComment(item.id)" class="btn btn-outline-dark me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="按讚">
                                </button>
                            </template>
                            <template v-else>
                                <p class="card-text me-1 fs-5"><i class="fa-solid fa-thumbs-up"></i></p>
                            </template>
                            
                            <p class="card-text me-3 fs-5 align-self-center"> {{item.totalThumbs}}</p>
                            <button v-if="item.status===isReplyed" type="button" class="btn btn-primary position-relative" data-bs-toggle="collapse"
                                    :data-bs-target='"#collapseExample"+index' @showReply(item.id)>
                                看回覆訊息
                            </button>
                        </div>

                        <template v-if="item.status===isReplyed">

                            <div class="collapse mt-3" :id='"collapseExample" + index'>
                                <div class="card card-body">
                                    <h5>來自{{item.hotelName}}的回覆:</h5>
                                    <p class="fs-6">{{item.replyMessage}}</p>
                                    <p class="card-text text-end">{{item.replyDate}}</p>
                                </div>
                            </div>
                        </template>
                    </div>


                    
                </div>
            </div>                               
        </div>

        <!-- Modal新增評論 -->
        <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="staticBackdropLabel">新增評論</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" @click="clearWords()"></button>
                    </div>
                    <div class="modal-body row">
                        <div class="col-md-10">
                        <div :class="{ 'warnShow': showAlert}">
                                評分必填*
                            </div>
                             <div class="mb-2 d-flex align-items-start">
                                <div class="position-relative">
                                    <input v-model="starpoint" id="target" type="range" :min="sliderMin" :max="sliderMax" step="1" list="tickmarks" @input="colorShift"/>
                                <datalist id="tickmarks" class="d-flex justify-content-between" style="width:200px;">
                                    <option value="1" label="1" class="ps-2"></option>
                                    <option value="5" label="5" class="pe-2"></option>
                                    <option value="10" label="10" class="ps-2"></option>
                                </datalist>
                              </div>
                                <input v-model="starpoint" type="number" class="form-control number-input" min="1" max="10" step="1" placeholder="評分*">
                            </div>
                            <div class="mb-3">
                                <input v-model="title" type="text" class="form-control" id="title" placeholder="評論標題">
                            </div>
                            <div class="mb-3">
                                <textarea v-model="pros" class="form-control" placeholder="優點" id="advantage" style="height: 200px"></textarea>
                            </div>
                            <div class="mb-3">
                                <textarea v-model="cons" class="form-control" placeholder="缺點" id="drawBacks" style="height: 200px"></textarea>
                            </div>
                            <div class="input-group mb-3">
                                <label class="btn btn-outline-success" for="file1" id="filesUpload">選擇檔案</label>
                                <input type="file" class="form-control d-none" id="file1" multiple="multiple" accept="image/*" @change="selectFiles">
                                <input v-model="fileName" type="text" class="form-control" aria-describedby="filesUpload" readonly>
                            </div>
                        </div>
                        


                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="shutDown" @click="clearWords()">關閉</button>
                        <button type="button" class="btn btn-primary" @click="sendForm">送出</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- Modal carousel -->
        <div class="modal fade bg-black" id="imgModal" tabindex="-1" aria-labelledby="imgModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div id="carouselImg" class="carousel carousel-dark slide" data-bs-ride="carousel">
                            <div class="carousel-inner">
                                <div :class="{ 'carousel-item': true, 'active': num === 0 }" v-for="(photo,num) in carouselList" :key="num">
                                    <img v-bind:src="getImgPath(photo)" class="d-block w-100">
                                </div>
                               
                            </div>
                            <button class="carousel-control-prev" type="button" data-bs-target="#carouselImg" data-bs-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                <span class="visually-hidden">Previous</span>
                            </button>
                            <button class="carousel-control-next" type="button" data-bs-target="#carouselImg" data-bs-slide="next">
                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                <span class="visually-hidden">Next</span>
                            </button>

                            <div class="carousel-indicators">
                                <button type="button" v-for="(photo, num) in carouselList"
                                        :key="num"
                                        data-bs-target="#carouselImg"
                                        :data-bs-slide-to="num"
                                        :class="{'active': num === 0}">
                                </button>

                            </div>
                        </div>
                       
                    </div>
                    
                </div>
            </div>
        </div>`
                         
       


};
