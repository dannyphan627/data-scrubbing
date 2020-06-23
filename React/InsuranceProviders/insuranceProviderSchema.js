import * as Yup from "yup";

const validateInsurance = Yup.object().shape({
  name: Yup.string()
    .min(2, "Must be 2 characters or more.")
    .max(200)
    .required(),
  siteUrl: Yup.string()
    .min(2, "Must be 2 characters or more.")
    .max(255)
});

export { validateInsurance };
