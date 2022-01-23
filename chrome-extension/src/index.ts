import getXpath from 'get-xpath'
//import getXpath = require('get-xpath');

console.log("test");

document.addEventListener("DOMContentLoaded", function () {
  const clickme = document.querySelector("#click-me");
  console.log(clickme);

  if (clickme !== null) clickme.addEventListener("click", sth);

  function sth() {
    const testInput = document.querySelector("#testInput");
    const toShow = document.querySelector("#boxToShow");

    if (toShow !== null) toShow.innerHTML = getXpath(testInput);

    console.log("firedddd");
  }
});
