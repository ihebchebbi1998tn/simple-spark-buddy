# Unit Testing with Jest

## Environment

The unit tests root folder should be the Crm.Web folder, not the Crm.Test, because of resource
availability, `node_modules` folder only available there.

The tests are running under `Node.js` environment with is extended with JSDom to fake browser API. There might be some
complex API calls, which need polyfill to work. There are two major API-s, which already has prepared polyfill in the
code, one is `canvas` and the other is `websql`.

We are using [Jest](https://jestjs.io/docs/using-matchers) version 27, ESM still experimental, because `Node.js` ESM
support is not quite there just yet, so the tests preferred to be written in CommonJS. This means in practical sense,
you dont use `import` rather than use `require()`.
(If you unsure what this mean, read after how `Node.js` works).

Jest has a plugin in place to compile Typescript automatically, so everything under the sun can be tested. Be aware, if
you opting in for Typescript, it will be type checked!

**Naming conventions:**

+ Name the test the same as your file what you want to test
+ If you testing a general API make some resemblance in name what are you trying to test at least
+ File ending **must** contain `spec` or `test` before the filetype, this is how Jest picks up what is test what is not
+ File types enabled by default, `js`, `jsx`, `ts`, `tsx`


## Matchers overview

Reference for all matchers, [Jest Matchers](https://jestjs.io/docs/using-matchers)

- `expect(...).toBe(...)`, runs `===` check
- `expect(...).toEqual(...)`, runs deep check, used in object
- `expect(...).toHaveLength(...)`, calls `Array.length` then checks it with `===`
- `expect(...).toBeTruthy()`, runs `==`
- `expect(...).toBeFalsy()`, runs `==`
- `expect(...).not.toBe()`, runs a `!(...)` with the chained function

## How to create a unit test

1. Under the `Crm.Web/Content/tests/` create your test `MyNewUnitTest.spec.js`.
2. Create the Test suit. You can use `function() {}` as well.
    ```
    describe("MyNewUnitTest", () => {

    });
    ```

3. Create your test case inside, use the best matcher according to your needs.

    ```
    describe("MyNewUnitTest", () => {
        test("my creatively named test", () => {
            const a = 2 + 2;
            expect(a).toBe(4);
        })
    });
    ```

4. Run it with `npx jest .\Content\tests\MyNewUnitTest.spec.js`
    - or if using Rider, you will see a green arrow next to your test
    - if you using VS, install VSCode, and read the following [article](https://jestjs.io/docs/troubleshooting)

## Tests with imports

1. Import your function
    1. If its using exports

    ```
    const HelperString = require("relative/path/to/the/file").HelperString;
    ```

    2. If its writing the window, a.k.a with side-effects
   ```
    require("relative/path/to/the/file");
    ```

2. Test your code

```
test("with imported function", () => {
    expect(importedAdder(2, 2)).toBe(4);
    expect(window.Adder(2, 2)).toBe(4);
})
```

## Test with Promises

There is 2 main ways to do it

- Done Callback, you provide a callback on top of the function, which will terminates the test. If you forget to call it, the test will time out.

    ```
    test("my second creative test", (done) => {
        window.Helper.Database.initialize().then(() => {
            expect(true).toEqual(false);
            done()
        })
    })
    ```

- Async keyword, you must use async/await in pairs!

   ```
   test("my third creative test", async () => {
        await window.Helper.Database.initialize()
        expect(true).toEqual(false);
    })
   ```

Note, that you cannot create an `async (done) => {}` test, one or the other.
