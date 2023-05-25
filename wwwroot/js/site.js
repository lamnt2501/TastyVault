// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const cookStepList = document.querySelector(".cookstep-list");
const AddCookStep =()=> {
  const formGroup = document.createElement("div");
  formGroup.classList.add("form-group");
  const label2 = document.createElement("label");
  label2.innerText = "Bước số " + ++cookStepList.childElementCount;
  const inputStepDes = document.createElement("textarea");
  inputStepDes.classList.add("form-control");
  inputStepDes.setAttribute("name","cs" + ++cookStepList.childElementCount)

  formGroup.appendChild(label2);
  formGroup.appendChild(inputStepDes);

  cookStepList.appendChild(formGroup);
}