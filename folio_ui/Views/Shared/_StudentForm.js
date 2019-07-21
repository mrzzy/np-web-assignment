/*
 * NP Web Assignment
 * Student Form Js
*/

import "./_StudentForm.css"

import "@yaireo/tagify/dist/tagify.css";
import Tagify from "@yaireo/tagify"
import API from "../API.js"

class StudentForm {
    // construct a student form  object
    constructor() {
        // load html elements
        this.formElement = $("#student-form");
        this.messager = $("#student-form-message");
        this.api = new API();
        this.student = null;
    }

    /* setup & prepration */
    // pull required data for form
    async pullData() {
        if(this.mode === "Edit") {
            // pull existing student data 
            const user = await this.api.getUser();   
            const response = await this.api.call("GET", "/api/student/" + user.id);
        
            if(response.status == API.status.success) {
                this.student = JSON.parse(response.content);
            }
        }

        // pull data async to speed up load speed
        var promises = [];
        // pull mentors data
        promises.push((async () =>  {
            const response = await this.api.call("GET", "/api/lecturers?names=1");
            this.mentors = JSON.parse(response.content);
        })());

        if(this.mode === "Edit") {
            promises.push((async () => {
                const response = await this.api.call("GET", 
                    "/api/lecturers?names=1&student=" + this.student.studentId);
                this.student.mentor = JSON.parse(response.content)[0];
            })());
        }
        
        // pull skillsets data
        promises.push((async () => {
            const response = await this.api.call("GET", "/api/skillsets?names=1");
            this.skillsets = JSON.parse(response.content);
        })())

        if(this.mode == "Edit") {
            promises.push((async () => {
                const response = await this.api.call("GET", 
                    "/api/skillsets?names=1&student=" + this.student.studentId);
                this.student.skillsets = JSON.parse(response.content);
            })());
        }
    
        await Promise.all(promises);
    }

    // configure mentor field in form with the given mentors
    configureMentor(mentors) {
        const input = $("#student-mentor").get(0);
        // populate mentor options list
        const mentorNames = this.mentors.map((m) => m.name);
    
        // setup tagify on input field
        const config = {
            whitelist: mentorNames,
            maxTags: 1,
            dropdown: {
                enabled: 0
            }
        }
        new Tagify(input, config);
    
    }

    // configure skillsets field in form with the given skillsets
    configureSkillsets(skillsets) {
        const input = $("#student-skillsets").get(0);
        const skillsetNames = skillsets.map((ss) => ss.name);

        // setup tagify on input field
        const config = {
            whitelist: skillsetNames,
            dropdown: {
                enabled: 0
            }
        }
        const tagify = new Tagify(input, config);
    
        // removal of skillsets from students happens on the fly
        if(this.mode === "Edit") {
            tagify.on("remove", (e) => {
                const skillsetName = e.detail.data.value;
                const skillset = this.skillsets
                    .filter(ss => ss.name == skillsetName)[0];
                if(skillset != null) {
                    this.api.call("POST", "/api/skillset/remove/" + skillset.id
                        + "?student=" + this.student.studentId);
                }
            })
        }
    }

    // configure on the fly  client side validation
    configureValidation() {
        $("[name]").blur((event) => {
            // mark event target input as touched
            $(event.target).data("touched", "true");
            // clear previous erros and  client side validation
            this.clearShown();
            this.validate("touched");
        });
    }

    // configure & prepare the form to operate in the given mode:
    // "Edit" for editing the current logged in student
    // "Create" for creating student
    async configure(mode) {
        this.mode = mode;

        // pull data
        await this.pullData();
    
        if(mode === "Edit") {
            // load existing student data info form
            this.load();
            
        } else if(mode == "Create") {
            // setup default values for password
            $("input[type='password']")
                .val("p@55Student")
                .change();
        } else {
            throw "StudentForm: Unknown mode: " + mode;
        }

        // configure mentor field
        this.configureMentor(this.mentors);

        // configure skillsets field
        this.configureSkillsets(this.skillsets);

        // configure on the fly validation for touched inputs
        this.configureValidation();
    
        // show configured form
        this.formElement.toggleClass("d-none");
    }
    
    /* validation */
    // validate the data in the current form, returning 
    // true if the data in the given form is valid, false otherwise
    // method: 
    // "all" - validate all fields no matter if the field has been touched
    // "touched" - validate only touched fields
    // interacted with.
    validate(method="all") {
        if(method != "all" && method != "touched") {
            throw "Unknown validation method " + method;
        }

        // convinence func to check method before showing
        const methodShow = (input, name, message) => {
            // check method for showing the error message
            if(method == "all" || (method == "touched"  
                &&  $(input).data("touched") === "true")) {
                this.show(name, message);
            }
        }
                
        var isValid = true;
        // check if required fields have content
        $("[required]").each((_, input) => {
            // .trim() to ignore whitespace
            if($(input).val().trim().length <= 0) {
                const name = $(input).attr("name");
                methodShow(input, name, name + " is required");
                isValid = false;
            }
        });

        // check if passwords match
        const password =  $("#student-password").val();
        const confirmPassword = $("#student-confirm-password").val();
        if(password.length > 1 && password !== confirmPassword)
        {
            methodShow($("#student-password").get(0),
                "password", "Passwords do not match");
            isValid = false;
        }

        // check if mentor selected is valid
        if(this.extractMentor() == null) {
            methodShow($("#student-mentor").get(0),
                "mentorId", "Name a single lecturer to be your mentor");
            isValid = false;
        }

        if(isValid === false) this.scrollFirstShown();

        return isValid;
    }

    
    /* submiting data */
    // submit the contents of this form, performing the action as as configured
    // by configure()
    async submit() {
        // clear previous errors
        this.clearShown();
        // client side validation 
        if(this.validate("all") == false) return;
        
        // commit changes using API
        const student = this.extract();
        if(student == null) return; // failed to extract student

        var route = null;
        if(this.mode === "Edit") {
            route = "/api/student/update/" + this.student.studentId;
        } else if (this.mode === "Create") {
            route = "/api/student/create";
        }
            
        const response = await this.api.call("POST", route, 
            { "Content-Type": "application/json"}, JSON.stringify(student));

        // check if call went through successful 
        // otherwise display errors
        if(response.status != API.status.success)
        {
            // load errors into form
            const errors = JSON.parse(response.content);
            this.showErrors(errors);
            return;
        }

        // save student as new student mode & update id
        const receipt = JSON.parse(response.content);
        student.studentId = receipt.id;
        this.student = student;
        
        // submit the students skillsets
        this.submitSkillsets(student, student.skillsets);
    }

    // submit the given skillsets, assigning them to the given student
    submitSkillsets(student, skillsets) {
        skillsets.map((skillset) => {
            this.api.call("POST", "/api/skillset/assign/" + skillset.id
                + "?student=" + student.studentId);
        });
    }

    /* loading & extracting data */
    // loads the data for editing this.student
    load() {
        for(const [name, value] of Object.entries(this.student))
        {
            // find input field that matches name of student property
            const input = $("[name='" + name + "']");
            if(input.length >= 1) {
                // populate field with value
                input.val(value);
                input.change(); // required to render input fields properly
            }
        }
        
        // load mentor into form 
        $("#student-mentor").val(this.student.mentor.name);

        // load skillsets into form
        const skillsetNames = this.student.skillsets.map(ss => ss.name);
        $("#student-skillsets").val(skillsetNames.join(","));
    }

    // extract & return  the mentor from the mentor form field
    // Return null if cannot extract mentor
    extractMentor() {
        const input = $("#student-mentor").val().trim();
        if(input.length <= 0) return null;
        const tags = JSON.parse(input);
        const mentorName = tags[0].value;
        const mentor = this.mentors.filter((m) => m.name == mentorName)[0]

        return mentor;
    }

    // extract & return the skillsets form the skilsets form field 
    // Return null if cannot extract skillests
    extractSkillsets() {
        const input = $("#student-skillsets").val().trim();
        if(input.length <= 0) return null;
        const tags = JSON.parse(input);
        const skillsetNames = tags.map(t => t.value);
        const skillsets = this.skillsets
            .filter(ss => skillsetNames.includes(ss.name));

        return skillsets;
    }

    // extracts fields of the form to produce a student object
    // returns the extracted student or null if could not extract student
    extract() {
        // create or update existing student
        var student = null;
        if(this.mode === "Create") {
            student = {};
        } 
        else if(this.mode === "Edit" ) {
            student = this.student;
        }

        // extract student properties with named fields
        $("[name]").each((_, input) => {
            const name = $(input).attr("name");
            student[name] = $(input).val();
        });

        // extract mentor id for student
        student.mentor = this.extractMentor();
        student.mentorId = student.mentor.id
    
        // extract skillsets for student
        student.skillsets = this.extractSkillsets();

        // extract password
        const password =  $("#student-password").val();
        if(password.length >= 1) student.password = password;

        return student;
    }

    /* display error messages */  
    // show the given message underneath the input field with the given name
    show(name, message) {
        // add element to show message
        const inputSelector = "[name='" + name + "']";
        const inputParent = $(inputSelector).parents(".form-field");
        inputParent.append(
            $("#form-message-template")
                .clone()
                .removeClass("d-none") 
                .removeAttr("id")
                .addClass("form-message") 
                .text(message)
        );
    }

    // show the validation errors for the student
    showErrors(errors) {
        //extract & show message from eerors
        for(const [name, messages] of Object.entries(errors)) {
            // normalise names so that names match up with input name attributes 
            const normedName = name.charAt(0).toLowerCase() + name.substr(1);
            const message = messages[0]; // only show the first message 
            
            this.show(normedName, message);
        }

        this.scrollFirstShown();
    }

    // scroll such that the first messsage shown is in the viewport (if any)
    scrollFirstShown() {
        if($(".from-message").length >= 1) {
        // scroll the first error into view
            $(".form-message")
                .parents(".form-field")
                .get(0)
                .scrollIntoView({ 
                    behavior: 'smooth',
                    block: "center"
                });
        }
    }

    // clear all messages 
    clearShown() {
        $(".form-message").remove();
    }
}

(async () => {
    // run script for student form only student form exists
    if($("#student-form").length >= 1)
    {
        // setup form
        const form = new StudentForm();
        const mode = $("#form-mode").val();
        form.configure(mode);

        // register callbacks
        $("#btn-submit-student").click((e) => {
            e.preventDefault();
            form.formElement.submit();
        });

        form.formElement.submit((e) => {
            e.preventDefault();
            form.submit();
        });
    }
})();
