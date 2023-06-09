﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const RecipesList = document.querySelector(".recipes-list");
const AddRecipe = () => {
  // tạo div class form group
  const formGroup = document.createElement("div");
  formGroup.classList.add("form-group");

  // tạo label cho thẻ select bên dưới
  const label1 = document.createElement("label");
  label1.innerText = "Tên món ăn";

  // tạo thẻ select
  const select = document.createElement("select");
  select.setAttribute("name", "r-" + ++RecipesList.childElementCount);

  // tạo các option cho thẻ select, dữ liệu được lấy từ file recipe/create
  (JSON.parse(document.querySelector(".data-recipes").value)).forEach(r => {
    const option = document.createElement("option");
    option.setAttribute("value", r.Id);
    option.innerText = r.Name;
    select.appendChild(option);
  });
  // thêm các phần từ vào from group
  formGroup.appendChild(label1);
  formGroup.appendChild(select);

  RecipesList.appendChild(formGroup);
}

const DeleteRecipe = () => {
  RecipesList.removeChild(RecipesList.lastChild);
}

const cookStepList = document.querySelector(".cookstep-list");
const AddCookStep = () => {
  // tạo div có class form-group
  const formGroup = document.createElement("div");
  formGroup.classList.add("form-group");

  // tạo label cho input bên dưới
  const label = document.createElement("label");
  label.innerText = "Bước số " + ++cookStepList.childElementCount;

  // tạo input
  const inputStepDes = document.createElement("textarea");
  inputStepDes.classList.add("form-control");
  inputStepDes.setAttribute("name", "cs" + ++cookStepList.childElementCount)

  // thêm các phần tử vào trong form group
  formGroup.appendChild(label);
  formGroup.appendChild(inputStepDes);

  //thêm phàn tử vào list
  cookStepList.appendChild(formGroup);
}

const DeleteCookStep = () => {
  cookStepList.removeChild(cookStepList.lastChild);
}


const ingredientList = document.querySelector(".ingredient-list");

const AddIngredient = () => {
  // tạo div class form group
  const formGroup = document.createElement("div");
  formGroup.classList.add("form-group");

  // tạo label cho thẻ select bên dưới
  const label1 = document.createElement("label");
  label1.innerText = "Tên nguyên liệu";

  // tạo thẻ select
  const select = document.createElement("select");
  select.setAttribute("name", "i-" + ++ingredientList.childElementCount);

  // tạo các option cho thẻ select, dữ liệu được lấy từ file recipe/create
  (JSON.parse(document.querySelector(".data").value)).forEach(i => {
    const option = document.createElement("option");
    option.setAttribute("value", i.Id);
    option.innerText = i.Name;
    select.appendChild(option);
  });

  const label2 = document.createElement("label");
  label2.innerText = "Định lượng ";

  const input = document.createElement('input');
  input.setAttribute("name", "qi-" + ++ingredientList.childElementCount)

  // thêm các phần từ vào from group
  formGroup.appendChild(label1);
  formGroup.appendChild(select);
  formGroup.appendChild(label2);
  formGroup.appendChild(input);

  ingredientList.appendChild(formGroup);
}

const DeleteIngredient = () => {
  ingredientList.removeChild(ingredientList.lastChild);
}

let jsonImgs;
if (document.querySelector(".jsonImg") != null) {
  jsonImgs = JSON.parse(document.querySelector(".jsonImg").value);
}
const activerecipeImg = document.querySelector(".recipe-img-active-wrap img");
const recipeImgList = document.querySelectorAll(".recipe-img-item img");

recipeImgList.forEach(recipeImgItem => {
  recipeImgItem.addEventListener("click", () => activerecipeImg.src = recipeImgItem.src);
});

const recipeImgPrev = document.querySelector(".recipe-img-prev");
const recipeImgNext = document.querySelector(".recipe-img-next");

const repalcerecipeImgSrc = () => {
  for (let i = 0; i < (recipeImgList.length > 3 ? 3 : recipeImgList.length); i++) {
    recipeImgList[i].src = "http://localhost:5271/" + jsonImgs[i].Path;
  }
}
recipeImgPrev?.addEventListener("click", () => {
  const lastImg = jsonImgs[jsonImgs.length - 1];
  for (let i = jsonImgs.length - 1; i > 0; i--) {
    jsonImgs[i] = jsonImgs[i - 1];
  }
  jsonImgs[0] = lastImg;;
  console.log(jsonImgs)
  repalcerecipeImgSrc();
});

recipeImgNext?.addEventListener("click", () => {
  const firstImg = jsonImgs[0];
  for (let i = 0; i < jsonImgs.length - 1; i++) {
    jsonImgs[i] = jsonImgs[i + 1];
  }
  jsonImgs[jsonImgs.length - 1] = firstImg;;

  repalcerecipeImgSrc();
});

setInterval(() => {
  for (let i = 0; i < jsonImgs?.length; i++) {
    if (activerecipeImg.src.includes(jsonImgs[i].Path.replace("\\", "/"))) {
      activerecipeImg.src = "http://localhost:5271/" + jsonImgs[i < jsonImgs.length - 1 ? (i + 1) : 0].Path;
      break;
    }
  }
}, 4000);

const toTopBtn = document.querySelector(".to-top");
window.onscroll = () => {
  if (scrollY >= 200) {
    toTopBtn.classList.remove("d-none");
  }
  else {
    toTopBtn.classList.add("d-none");
  }
}

const menuToggle = document.querySelector(".show-menu");
menuToggle.addEventListener("click", function () {
  document.querySelector(".menu-side-bar").classList.toggle("show-menu");
});

// --------------------------
// var multipleCardCarousel = document.querySelector(
//   "#carouselExampleControls"
// );
// if (window.matchMedia("(min-width: 768px)").matches) {
//   var carousel = new bootstrap.Carousel(multipleCardCarousel, {
//     interval: false,
//   });
//   var carouselWidth = $(".carousel-inner")[0].scrollWidth;
//   var cardWidth = $(".carousel-item").width();
//   var scrollPosition = 0;
//   $("#carouselExampleControls .carousel-control-next").on("click", function () {
//     if (scrollPosition < carouselWidth - cardWidth * 4) {
//       scrollPosition += cardWidth;
//       $("#carouselExampleControls .carousel-inner").animate(
//         { scrollLeft: scrollPosition },
//         600
//       );
//     }
//   });
//   $("#carouselExampleControls .carousel-control-prev").on("click", function () {
//     if (scrollPosition > 0) {
//       scrollPosition -= cardWidth;
//       $("#carouselExampleControls .carousel-inner").animate(
//         { scrollLeft: scrollPosition },
//         600
//       );
//     }
//   });
// } else {
//   $(multipleCardCarousel).addClass("slide");
// }

