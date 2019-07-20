/*
 * NP Web Assignment
 * Student Form Js
*/

import API from "../API.js"

class StudentForm {
    // construct a student form  object
    constructor() {
        // load html elements
        this.form = $("#student-form");
        this.messager = $("#student-form-message");

        this.api = new API();
        this.student = null;
    }

    // configure the form to operate in the given mode:
    // "Edit" for editing the current logged in student
    // "Create" for creating student
    async configure(mode) {
        if(mode === "Edit") {
            // pull existing student data 
            const user = await this.api.getUser();   
            const response = await this.api.call("GET", "/api/student/" + user.id);
            // load existing student data info form
            if(response.status == API.status.success) {
                this.student = JSON.parse(response.content);
                this.load(this.student);
            }
        } else if(mode == "Create") {
            // do nothing
        } else {
            throw "StudentForm: Unknown mode: " + mode;
        }

        // show configured form
        this.form.toggleClass("d-none");
    }

    // loads the given student object into the form
    load(student) {
        for(const [name, value] of Object.entries(student))
        {
            console.log(name);
            // find input field that matches name of student property
            const input = $("[name='" + name.toLowerCase() + "']");
            if(input.length >= 1) {
                // populate field with value
                input.val(value);
                input.change(); // required to render input fields properly
            }
        }
    }
}

$(document).ready(async () => {
    // run script for student form only student form exists
    if($("#student-form").length >= 1)
    {
        const form = new StudentForm();
        const mode = $("#form-mode").val();
        await form.configure(mode);
    }
});
