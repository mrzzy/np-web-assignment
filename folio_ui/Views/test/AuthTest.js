/*
 * Authentication test
 * JS
*/

import Auth from "../Auth.js"
import assert from "assert";

// tests for Auth class
describe("Auth", () => {
    // tests if Auth.login() can perform login
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".login()", () => {
        const auth = new Auth(process.env.API_HOST);
        it("should perform login successfully and return true", async () => {
            const result = await auth.login("s1234112@ap.edu.sg", "p@55Student");
            assert.equal(result, true);
        });
    });

    // tests if Auth.check() can check login token
    // NOTE: requires existing data in the database as defined in setup SQL
    describe(".check()", () => {
        const auth = new Auth(process.env.API_HOST);
        auth.login("s1234112@ap.edu.sg", "p@55Student");

        it("should check auth token successfully", async () => {
            const result = await auth.check();
            assert.equal(result, true);
        });
    });
});
