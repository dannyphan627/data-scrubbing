import React from "react";
import * as insuranceHelper from "../../services/insuranceProviderService";
import logger from "sabio-debug";
import { Form, FormGroup, Label, Button } from "reactstrap";
import { Formik, Field } from "formik";
import * as schema from "./insuranceProviderSchema";
import Swal from "sweetalert2";
import PropTypes from "prop-types";

const _logger = logger.extend("InsuranceProviderForm");

class InsuranceProviderForm extends React.Component {
  state = {
    formData: {
      id: "",
      name: "",
      siteUrl: ""
    }
  };

  componentDidMount() {
    if (this.props.match.params.id) {
      let id = parseInt(this.props.match.params.id);
      insuranceHelper
        .getInsuranceById(id)
        .then(this.onGetIdSuccess)
        .catch(this.onGenericFail);
    }
  }

  handleSubmit = values => {
    const provider = {
      id: this.state.formData.id,
      name: values.name,
      siteUrl: values.siteUrl
    };

    if (this.state.formData.id) {
      insuranceHelper
        .updateInsuranceProvider(provider)
        .then(this.onSaveSuccess)
        .catch(this.onGenericFail);
    } else {
      insuranceHelper
        .addInsuranceProvider(provider)
        .then(this.onSaveSuccess)
        .catch(this.onGenericFail);
    }
  };

  handleCancel = () => {
    this.props.history.push("/insurances");
  };

  onGetIdSuccess = response => {
    this.setState(prevState => {
      return {
        ...prevState,
        formData: response.item
      };
    });
  };

  onSaveSuccess = response => {
    _logger(response);
    this.props.history.push("/insurances");
    Swal.fire("Success", "Save was successful!", "success");
  };

  onGenericFail = errResponse => {
    _logger(errResponse);
    Swal.fire("Error", "Something went wrong.", "error");
  };

  render() {
    return (
      <React.Fragment>
        <Formik
          enableReinitialize={true}
          validationSchema={schema.validateInsurance}
          initialValues={this.state.formData}
          onSubmit={this.handleSubmit}
        >
          {props => {
            const {
              values,
              touched,
              errors,
              handleSubmit,
              isValid,
              isSubmitting
            } = props;
            return (
              <div className="card">
                <div className="bg-light card-body">
                  <div className="mb-0 card-title">Insurance Provider Info</div>
                </div>
                <Form onSubmit={handleSubmit} className={"card-body"}>
                  <div className="row">
                    <div className="col-md-8">
                      <FormGroup>
                        <Label>Name</Label>
                        <Field
                          name="name"
                          type="text"
                          placeholder="Provider Name"
                          autoComplete="off"
                          values={values.name}
                          className={
                            errors.name && touched.name
                              ? "form-control error"
                              : "form-control"
                          }
                        />
                        {errors.name && touched.name && (
                          <span className="input-feedback">{errors.name}</span>
                        )}
                      </FormGroup>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-8">
                      <FormGroup>
                        <Label>Website</Label>
                        <Field
                          name="siteUrl"
                          type="url"
                          values={values.siteUrl}
                          placeholder="Site Url"
                          autoComplete="off"
                          className={
                            errors.siteUrl && touched.siteUrl
                              ? "form-control error"
                              : "form-control"
                          }
                        />
                        {errors.siteUrl && touched.siteUrl && (
                          <span className="input-feedback">
                            {errors.siteUrl}
                          </span>
                        )}
                      </FormGroup>
                    </div>
                  </div>
                  <Button
                    type="submit"
                    disabled={!isValid || isSubmitting}
                    className="btn btn-success btn-secondary"
                  >
                    Submit
                  </Button>
                  <Button
                    type="button"
                    className="btn btn-dark ml-2 btn-secondary"
                    onClick={this.handleCancel}
                  >
                    Cancel
                  </Button>
                </Form>
              </div>
            );
          }}
        </Formik>
      </React.Fragment>
    );
  }
}

InsuranceProviderForm.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string.isRequired
    })
  })
};

export default InsuranceProviderForm;
