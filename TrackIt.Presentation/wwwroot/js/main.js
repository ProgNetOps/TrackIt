/*==================== MENU SHOW Y HIDDEN ====================*/
const navMenu = document.getElementById("nav-menu"),
  navToggle = document.getElementById("nav-toggle"),
  navClose = document.getElementById("nav-close");

/*===== MENU SHOW =====*/
/* Validate if constant exists */
if (navToggle) {
  navToggle.addEventListener("click", () => {
    navMenu.classList.add("show-menu");
  });
}

/*===== MENU HIDDEN =====*/
/* Validate if constant exists */
if (navClose) {
  navClose.addEventListener("click", () => {
    navMenu.classList.remove("show-menu");
  });
}
/*==================== REMOVE MENU MOBILE ====================*/
const navLink = document.querySelectorAll(".nav__link");

function linkAction() {
  const navMenu = document.getElementById("nav-menu");
  //When we click on each nav link, we remove the show-menu class
  navMenu.classList.remove("show-menu");
}
navLink.forEach((n) => n.addEventListener("click", linkAction));


/*==================== SERVICES MODAL ====================*/
//const modalViews = document.querySelectorAll(".services__modal"),
//modalBtns = document.querySelectorAll("span.services__button"),
//modalCloses = document.querySelectorAll(".services__modal-close")

//let modal = function(modalClick){
//  modalViews[modalClick].classList.add("active-modal")
//}

//modalBtns.forEach((modalBtns,i) =>{
//  modalBtns.addEventListener("click", () =>{
//    modal(i)
//  })
//})

//modalCloses.forEach((modalClose) =>{
//  modalClose.addEventListener("click", () =>{
//      modalViews.forEach((modalView) =>{
//        modalView.classList.remove("active-modal")
//      })
//  })
//})

/*==================== PORTFOLIO SWIPER  ====================*/
let swiperPortfolio = new Swiper(".portfolio__container",{
  cssMode:true,
  loop:true,

  navigation:{
    nextEl:".swiper-button-next",
    prevEl:".swiper-button-prev",
  },
  pagination:{
    el: ".swiper-pagination",
    clickable: true,
  },  
});

/*==================== TESTIMONIAL ====================*/
let swiperTestimonial = new Swiper(".testimonial__container",{
  loop:true,
  grabCursor:true, //Makes the swiping by hand possible
  spaceBetween:48,

  pagination:{
    el: ".swiper-pagination",
    clickable: true,
    dynamicBullets: true
  },
  breakpoints:{
    568:{
      slidesPerView:2,
    }
  }  
});

/*========= SCROLL SECTIONS ACTIVE LINK ==========*/

const sections = document.querySelectorAll("section[id]")

function scrollActive(){
  const scrollY = window.scrollY;

  sections.forEach(current => {
    const sectionHeight = current.offsetHeight;
    const sectionTop = current.offsetTop = 50;
    sectionId = current.getAttribute("id");

    if(scrollY > sectionTop && scrollY <= sectionTop + sectionHeight){
      document.querySelector(".nav__menu a[href*=" + sectionId 
      + "]").classList;
    }
    else{
      document.querySelector(".nav__menu a[href*=" + sectionId 
      + "]").classList;
    }
  })
}
window.addEventListener("scroll", scrollActive)


/*==================== CHANGE BACKGROUND HEADER ====================*/

function scrollHeader(){
  const nav = document.getElementById("header")
  //When the scroll is greater than 200 ciewport height, add the scroll-header class to the header tag
  if(this.scrollY >= 80){
    nav.classList.add("scroll-header");
  }
  else{
    nav.classList.remove("scroll-header");
  }
}
window.addEventListener("scroll",scrollHeader)


//=====================================================//
//============= SCROLL UP START=================//
//=====================================================//

function scrollUp(){
  const scrollUp = document.getElementById("scroll-up");
  //When the scroll is higher than 560 viewport height, add the show-scroll class to 
  if(this.scrollY >= 560){
    scrollUp.classList.add("show-scroll");
  }
  else{
    scrollUp.classList.remove("show-scroll");
  }
}
window.addEventListener("scroll", scrollUp)
//=====================================================//
//============= SCROLL UP ENDS=================//
//=====================================================//




//=====================================================//
//============= DARK LIGHT THEME START=================//
//=====================================================//
const themeButton = document.getElementById("theme-button");
const darkTheme = "dark-theme";
const iconTheme = "uil-sun";

//Previously selected topic (if user selected)
const selectedTheme = localStorage.getItem("selected-theme");
const selectedIcon = localStorage.getItem("selected-icon");

//We obtain the current theme that the interface has by validating the dark-theme class
const getCurrentTheme = () => document.body.classList.contains(darkTheme) ? "dark" : "light";
const getCurrentIcon = () => themeButton.classList.contains(iconTheme) ? "uil-moon" : "uil-sun";

//We validate if the user previously chose a topic

if(selectedTheme){
  //If the validation is fulfilled, we ask what the issue was to know if we activated or deactivated the dark
  document.body.classList[selectedTheme === "dark" ? "add" : "remove"](darkTheme);
  themeButton.classList[selectedIcon === "uil-moon" ? "add" : "remove"](iconTheme);
}

//Activate /Deactivate the theme manually with the button
themeButton.addEventListener("click", () =>{
  //Add or remove the dark/icon theme
  document.body.classList.toggle(darkTheme);
  themeButton.classList.toggle(iconTheme);
  //We save the theme and the current icon that the user chose
  localStorage.setItem("selected-theme",getCurrentTheme());
  localStorage.setItem("selected-icon",getCurrentIcon());
})

//=====================================================//
//============= DARK LIGHT THEME ENDS ================//
//=====================================================//





//=====================================================//
//============= STACKED CARDS START====================//
//=====================================================//

let cards = document.querySelectorAll(".stackedcard"),
    stackArea = document.querySelector(".stack-area");

function rotateCards(){
    let angle = 0;
    cards.forEach((card) =>{
        if(card.classList.contains("active")){
            card.style.transform = `translate(-50%,-120vh) rotate(-48deg)`;
        }
        else{
            card.style.transform = `translate(-50%,-50%) rotate(${angle}deg)`;
        angle = angle - 10;
        }
    });
}

rotateCards();

window.addEventListener("scroll", () => {
    let proportion = stackArea.getBoundingClientRect().top/window.innerHeight;
    if(proportion <= 0){
        let n = cards.length;
        let index = Math.ceil((proportion*n)/2);
        index = Math.abs(index) - 1;
        for(let i=0; i < n; i++){
            if(i <= index){
                cards[i].classList.add("active");
            }
            else{
                cards[i].classList.remove("active");    
            }
        }
        rotateCards();
    }
});

//=====================================================//
//============= STACKED CARDS ENDS ====================//
//=====================================================//


