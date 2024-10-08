/** @format */

function islogin() {
    const loginButton = document.getElementById("login");
    const userAccount = document.getElementById("userAccount");
    let userId = localStorage.getItem("UserId");
  
    if (userId != null) {
      loginButton.textContent = "الخروج";
      loginButton.addEventListener("click", function () {
        localStorage.removeItem("UserId");
        localStorage.removeItem("Token");
        alert("تم تسجيل خروجك بنجاح");
  
        loginButton.textContent = "تسجيل دخول";
  
        window.location.href = "login.html";
      });
    } else {
      loginButton.textContent = "تسجيل دخول";
      userAccount.style.display = "none";
      loginButton.addEventListener("click", function () {
        window.location.href = "login.html";
      });
    }
  }
  
  islogin();
  