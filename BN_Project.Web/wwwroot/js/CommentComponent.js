function LikeComment(element) {
    let id = element.id;
    $('#commentSection').load('/Home/LikeComment?commentId=' + id);
}
function DisLikeComment(element) {
    let id = element.id;
    $('#commentSection').load('/Home/DisLikeComment?commentId=' + id);
}
