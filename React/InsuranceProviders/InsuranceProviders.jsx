import React from "react";
//import InsuranceProviderCards from "./InsuranceProviderCard";
import * as insuranceProvService from "../../services/insuranceProviderService";
import logger from "sabio-debug";
import Swal from "sweetalert2";
import Pagination from "rc-pagination";
import localeInfo from 'rc-pagination/lib/locale/en_US';
import "rc-pagination/assets/index.css";
import PropTypes from "prop-types";
import "./InsuranceProviders.css";
import SearchBar from "../SearchBar";
import { Link } from "react-router-dom";
import InsuranceProviderRows from "./InsuranceProviderRows";

const _logger = logger.extend("InsuranceProviders");

class InsuranceProviders extends React.Component {
  state = {
    insuranceProviders: [],
    mappedProviders: [],
    pageIndex: 0,
    totalCount: 0,
    pageSize: 12,
    searchResult: ""
  };

  componentDidMount() {
    this.getAll(this.state.pageIndex);
  }

  getAll = index => {
    insuranceProvService
      .getAllInsuranceProvs(index, this.state.pageSize)
      .then(this.onGetSuccess)
      .catch(this.onGenericFail);
  };

  onGetSuccess = ({ item }) => {
    this.setState(() => {
      return {
        insuranceProviders: item.pagedItems,
        mappedProviders: item.pagedItems.map(this.mapInsuranceProvider),
        totalCount: item.totalCount,
        currentPage: item.pageIndex + 1
      };
    });
  };

  mapInsuranceProvider = provider => {
    return (
      <InsuranceProviderRows
        currentUserRole={this.props.currentUser.roles}
        provider={provider}
        key={"InsuranceProvider_" + provider.id}
        onDelete={this.handleDelete}
        onEdit={this.handleEdit}
      />
    );
  };

  handleEdit = provider => {
    this.props.history.push(`/insurances/${provider.id}/edit`);
  };

  onChangePage = page => {
    if (this.state.searchResult) {
      this.getSearchResults(page - 1);
    } else {
      this.getAll(page - 1);
      this.setState({ currentPage: page });
    }
  };

  onSearchChange = event => {
    this.setState({ searchResult: event, pageIndex: 0 }, () => {
      this.state.searchResult.length > 0
        ? this.getSearchResults(this.state.pageIndex)
        : this.getAll(this.state.pageIndex);
    });
  };

  getSearchResults = index => {
    insuranceProvService
      .searchInsuranceProvider(this.state.searchResult, index)
      .then(this.onSearchSuccess)
      .catch(this.onSearchFail);
  };

  onSearchSuccess = response => {
    this.setState(prevState => {
      return {
        prevState,
        mappedProviders: response.item.pagedItems.map(
          this.mapInsuranceProvider
        ),
        totalCount: response.item.totalCount,
        currentPage: response.item.pageindex + 1
      };
    });
  };

  handleDelete = provider => {
    Swal.fire({
      title: "Are you sure?",
      text: "This can't be undone.",
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#DD6B55",
      confirmButtonText: "Yes, delete it!",
      cancelButtonText: "No, cancel please!",
      closeOnConfirm: false,
      closeOnCancel: false
    }).then(result => {
      if (result.value) {
        insuranceProvService
          .deleteInsuranceProvider(provider)
          .then(this.onDeleteSuccess)
          .catch(this.onGenericFail);
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire("Cancelled", "", "error");
      }
    });
  };

  onDeleteSuccess = response => {
    Swal.fire("Deleted!", "Provider has been deleted.", "success");
    this.setState(prevState => {
      const newArray = prevState.insuranceProviders.filter(
        item => item.id !== response.payload.id
      );

      return {
        mappedProviders: newArray.map(this.mapInsuranceProvider)
      };
    });
  };

  onGenericFail = errResponse => {
    _logger(errResponse);
    Swal.fire("Error", "Something wrong happened.", "error");
  };

  onSearchFail = errResponse => {
    _logger(errResponse);
    this.setState({ mappedProviders: null });
  };

  renderAddButton = () => {
    return this.props.currentUser.roles.includes("SysAdmin") ? (
      <div className="btn-group btn-group-sm">
        <Link to="/insurances/new">
          <button
            type="button"
            className="btn btn-ins-prov btn-success btn-primary createBtn"
          >
            <span>
              <i className="fa glyphicon glyphicon-plus fa-plus" /> New
            </span>
          </button>
        </Link>
      </div>
    ) : (
        ""
      );
  };

  renderHeaderColumns = () => {
    return this.props.currentUser.roles.includes("SysAdmin") ? (
      <thead className="table-header-wrapper">
        <tr>
          <th className="p-1">Name</th>
          <th className="p-1">Website</th>
          <th className="p-1">Edit</th>
          <th className="p-1">Delete</th>
        </tr>
      </thead>
    ) : (
        <thead className="table-header-wrapper">
          <tr>
            <th className="p-1">Name</th>
            <th className="p-1">Website</th>
          </tr>
        </thead>
      );
  };

  render() {
    return (
      <div>
        <div className="row">
          <div className="col-md-12">
            <div className="card">
              <div
                className="m-0 p-3 border-bottom bg-light card-title"
                style={{ fontWeight: "500", width: "100%" }}
              >
                Insurance Providers
              </div>
              <div className="card-body">
                <div className="react-bs-table-container">
                  <div className="react-bs-table-tool-bar">
                    <div className="row">
                      <div className="col-sm-6 ">{this.renderAddButton()}</div>
                      <div className="col-sm-6">
                        <SearchBar onChange={this.onSearchChange} />
                      </div>
                    </div>
                  </div>
                  <div className="tab-content mt-3 text-center">
                    <div className="tab-pane active">
                      <div className="row">
                        <div className="col-sm-12">
                          <div>
                            <div className="table-responsive">
                              <table className="v-middle table table-striped table-bordered table-hover">
                                {this.renderHeaderColumns()}
                                <tbody>{this.state.mappedProviders}</tbody>
                              </table>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="react-bs-table-pagination">
                    <div
                      className="row justify-content-center"
                      style={{ marginTop: 15 }}
                    >
                      <div style={{ display: "block" }}>
                        <Pagination
                          className="-pageInfo"
                          defaultCurrent={this.state.pageIndex}
                          total={this.state.totalCount}
                          onChange={this.onChangePage}
                          pageSize={this.state.pageSize}
                          locale={localeInfo}
                        />
                      </div>
                    </div>
                  </div>
                  <div className="s-alert-wrapper" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

InsuranceProviders.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  location: PropTypes.shape({
    pathName: PropTypes.string
  }),
  math: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.number
    })
  }),
  currentUser: PropTypes.shape({
    roles: PropTypes.arrayOf(PropTypes.string)
  })
};

export default InsuranceProviders;
