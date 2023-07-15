const colors = require("tailwindcss/colors");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        primary: colors.amber["500"],
        secondary: "#56CCF2",
        "shopping-list": "#FFF0DE",
        "add-item": "#80485B",
        "somewhat-black": "#34333A",
      },
      fontFamily: {
        quicksand: ["Quicksand", "sans-serif"],
      },
    },
  },
  plugins: [require("@tailwindcss/forms")],
};