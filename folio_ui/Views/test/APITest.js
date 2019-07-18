/*
 * API Client tests
 * JS
*/

import API from "../API.js"
import assert from "assert";
import * as Cookies from "js-cookie";

// tests for API class
describe("API", () => {
    // tests if API.login() can perform login
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".login()", () => {
        const api = new API();
        it("should perform login successfully and return true", async () => {
            const result = await api.login("s1234112@ap.edu.sg", "p@55Student");
            assert.equal(result, true);
        });
    });

    // test if API.call() can make API calls 
    describe(".call()", () => {
        const api = new API();

        it("should make create & delete skillset api call successfully", async () => {
            // login as lecturer as only lecturers can create/delete skilsets
            await api.login("Peter_Ghim@ap.edu.sg", "p@55Mentor");
            
            // make create skillset api call
            const skillSet = { "skillSetName": "Fine Dining with $300 suits" };

            var response = await api.call(
                "POST", 
                "/api/skillset/create", 
                { "Content-Type": "application/json"},
                JSON.stringify(skillSet)
            );
            assert.equal(response.status, API.status.success)
            const skillSetId = JSON.parse(response.content).skillSetId;

            // make deletion api call
            response = await api.call(
                "POST",
                "/api/skillset/delete/" + skillSetId
            );
            assert.equal(response.status, API.status.success)
        });

    });

    /* Authorization & Authentication */
    // tests if API.check() can check login token
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".check()", () => {
        const api = new API();

        it("should check api token successfully", async () => {
            await api.login("s1234112@ap.edu.sg", "p@55Student");
            const result = await api.check();
            assert.equal(result, true);
        });
    });
    
    // tests if API.getUser() can obtain user infomation
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".getUser()",() => {
        const api = new API();

        it("should obtain user infomation of currently authenticated user", async () => {
            await api.login("s1234112@ap.edu.sg", "p@55Student");
            const userinfo = await api.getUser();
            assert.equal(userinfo.id, 2);
            assert.equal(userinfo.name, "Amy Ng");
            assert.equal(userinfo.userRole, "Student");
            assert.equal(userinfo.emailAddr, "s1234112@ap.edu.sg");
        });
    });

    // tests if API.logout() can perform logout cleanly
    describe(".logout()", () => {
        const api = new API();

        it("should logout cleanly", async () => {
            await api.login("s1234112@ap.edu.sg", "p@55Student");
            api.logout();
            assert.equal(api.token, null);
            assert.equal(Cookies.get("API.token"), null);
        });
    });
});
