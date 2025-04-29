// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Initialization for ES Users
import { Dropdown, initMDB } from "mdb-ui-kit";

initMDB({ Dropdown });

const triggers = [
    'primary',
    'secondary',
    'success',
    'danger',
    'warning',
    'info',
    'light',
    'dark',
];
const basicInstances = [
    'alert-primary',
    'alert-secondary',
    'alert-success',
    'alert-danger',
    'alert-warning',
    'alert-info',
    'alert-light',
    'alert-dark',
];

triggers.forEach((trigger, index) => {
    let basicInstance = mdb.Alert.getInstance(document.getElementById(basicInstances[index]));
    document.getElementById(trigger).addEventListener('click', () => {
        basicInstance.show();
    });
});