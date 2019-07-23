/*
 * Student Suggestions
 * NP Web Assignment
 * JS
*/

// image assets
import "./assets/notification_without_dot.png"
import "./assets/notifications_with_dot.png"
 
// styles
import "./_SuggestionsStudent.css"

// imports
import API from "../../API.js"

/* student suggestions */
// student suggestions support class
class SuggestionsView { 
    constructor() {
        this.api = new API();
    }

    // confgure the view to render suggestions
    async configure() { 
        this.clear();
        this.suggestions = await this.pull();
        this.suggestions = this.suggestions.filter((s) => s.status == "N");
        await this.render(this.suggestions);
    }

    // pull and return the suggestions for the current student
    // returns empty [] if unable to pull suggestions
    async pull() {
        const student = await this.api.getUser();
        // pull ids of suggestions for student from api
        const response = await this.api.call("GET", "/api/suggestions"
            + "?student=" + student.id);
        if(response.status != API.status.success) return  [];
        const ids = JSON.parse(response.content);

        // pull suggestions data for ids
        const promises = ids.map(async (id) => { 
            const response = await this.api.call("GET", "/api/suggestion/" + id);
            const suggestion = JSON.parse(response.content);
            return suggestion;
        });
        const suggestions = await Promise.all(promises);

        return suggestions;
    }

    // render the unacknowledged suggestions for the current student
    async render(suggestions) {
        this.renderIndicator(suggestions);

        for(const suggestion of suggestions) {

            // render suggestion using template suggestion element
            const template = $("#suggestion-template").get(0);
            const element = $(template).clone();

            // configure element 
            element
                .removeAttr("id")
                .removeClass("d-none")
                .addClass("d-flex")
                .addClass("rendered");
        
            // inject description into element
            element.find(".description").val(suggestion.description);
            // configure acknowlege button in element to acknowlege suggestion
            element.find("button.acknowlege").click(async () => {
                // acknowlege suggestion and remove element
                await this.acknowlege(suggestion);
                element.remove();
                this.renderIndicator(this.suggestions);
            
                // hide suggestion view if no more suggestions to show
                if(this.suggestions.length <= 0) {
                    this.hide();
                }
            });
        
            // add to suggestion list
            $(".suggestions-list").append(element);
        }
    }


    // render the indicator in  button
    renderIndicator(suggestions) {
        if(suggestions.length >= 1) {
            // render indicator  to indicate suggestions
            $("#button-show-suggestions .indicator").attr("src",
                "/img/notifications_with_dot.png");
        } else {
            // indicate no suggestions
            $("#button-show-suggestions .indicator").attr("src",
                "/img/notification_without_dot.png");
        }
    }

    // clear rendered suggestions
    clear() {
        $(".suggestions-list").find(".rendered").remove();
    }

    // acknowlege the given suggestion
    // returns the update suggestion
    async acknowlege(suggestion) {
        // mark suggestion as accepted using api
        await this.api.call("POST", "/api/suggestion/ack/" + suggestion.suggestionId);
        // update local data
        this.suggestions = this.suggestions.filter(
            (s) => s.suggestionId !== suggestion.suggestionId);

        return suggestion;
    }

    // show the suggestion view
    show() {
        $("#suggestions-container").addClass("d-flex");
        $("#suggestions-container").removeClass("d-none");
    }

    // hide the suggestion view
    hide() {
        $("#suggestions-container").removeClass("d-flex");
        $("#suggestions-container").addClass("d-none");
    }
}

(async () => {
    // only if run suggestions card present
    if($("#suggestions-container").length >= 1) {
        const suggestionView = new SuggestionsView();
        suggestionView.configure();
        
        $(".suggestions.card").click((event) => {
            event.stopPropagation();
        });
        
        // hide suggestions
        $("#suggestions-container").click(() => {
            suggestionView.hide();
        });

        // show suggestions
        $("#button-show-suggestions").click(() => {
            suggestionView.show();
        });
    }
})();
