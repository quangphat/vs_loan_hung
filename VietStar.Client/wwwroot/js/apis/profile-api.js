
function ProfileSearch(dateType = 1, group = 2) {
    fetch(`/profile/search?dateType=${dateType}&groupId=${group}`)
        .then(res => res.json())
        .then(res => {
            if (res.success == true) {
                console.log(res.data)
                return res.data
            }
            else {
                return null;
            }
        })
        .catch(err => {
            console.log("failed");
            return null;
        });
}