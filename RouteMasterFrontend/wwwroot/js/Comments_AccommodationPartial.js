var vueApp1 = {
    data() {
        return {
            indexVM: [],
            item: {},
            isReplyed: "已回復",
            ep: null,
            selected: 0,
            hotelId: 1, //假設這是呈現AccomodationId=2的評論清單，這裡直接賦值=2
            thumbicon: [],

        }
    },
    created: function () {
        let _this = this;
    },
    mounted: function () {
        let _this = this;
        _this.commentDisplay();
    },
    methods: {
        commentDisplay: function () {
            let _this = this;
            var request = {};
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

    }

};
const dec = Vue.createApp(vueApp1).mount("#dec");
