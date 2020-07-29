
function ProfileSearch(dateType = 1) {
    fetch('/profile/search?dateType=2')
        .then(res => res.json())
        .then(res => {
            console.log(`success`);
        });
}