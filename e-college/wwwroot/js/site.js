
function RegisterStudent() {
    debugger;
    var defaultBtnValue = $('#submit_btn').html();
    $('#submit_btn').html("Please wait...");
    $('#submit_btn').attr("disabled", true);

    var selectedUserId = [];
    $('.user-checkbox:checked').each(function () {
        selectedUserId.push($(this).data('user-id'));
    });

    var data = {};
    data.FirstName = $('#firstname').val();
    data.LastName = $('#lastname').val();
    data.UserName = $('#username').val();
    data.OtherName = $('#othername').val();
    data.Phonenumber = $('#phonenumber').val();
    data.Email = $('#email').val();
    data.Password = $('#password').val();
    data.ConfirmPassword = $('#confirmPassword').val();
    data.State = $('#state').val();
    data.Country = $('#country').val();
    data.Address = $('#address').val();
    data.DOB = $('#dateOfBirth').val();
    if (data.DOB == "") {
        data.DOB = "0001-01-01T00:00:00"
    };
    var edulevel = $('.user-checkbox').data('user-id');
    //var eduLevel = $(this).data('user-id');

    if (data.FirstName != "" && data.LastName != "" && data.UserName != "" && data.Phonenumber != ""
        && data.Email != "" && data.Password != "" && data.ConfirmPassword != "" && data.State != "" && data.Country != ""
        && data.Address != "") {
        let userDetails = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            url: '/Account/StudentRegistration',
            dataType: 'json',
            data:
            {
                userDetails: userDetails,
                edulevel: edulevel,
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    var url = '/Account/Login';
                    successAlertWithRedirect(result.msg, url);
                    $('#submit_btn').html(defaultBtnValue);
                }
                else {
                    $('#submit_btn').html(defaultBtnValue);
                    $('#submit_btn').attr("disabled", false);
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                $('#submit_btn').html(defaultBtnValue);
                $('#submit_btn').attr("disabled", false);
                errorAlert("Please check and try again. Contact Admin if issue persists..");
            },
        })
    } else {
        $('#submit_btn').html(defaultBtnValue);
        $('#submit_btn').attr("disabled", false);
        errorAlert("Please fill the form Correctly");
    };
}

function login() {
    var defaultBtnValue = $('#submit_btn').html();
    $('#submit_btn').html("Please wait...");
    $('#submit_btn').attr("disabled", true);

    var email = $('#email').val();
    var password = $('#password').val();
    $.ajax({
        type: 'Post',
        url: '/Account/Login',
        dataType: 'json',
        data:
        {
            email: email,
            password: password
        },
        success: function (result) {
            if (!result.isError) {
                var n = 1;
                localStorage.removeItem("on_load_counter");
                localStorage.setItem("on_load_counter", n);
                location.replace(result.dashboard);
                return;
            }
            else {
                $('#submit_btn').html(defaultBtnValue);
                $('#submit_btn').attr("disabled", false);
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            $('#submit_btn').html(defaultBtnValue);
            $('#submit_btn').attr("disabled", false);
            errorAlert("An error occured, please try again.");
        }
    });
}

function addSubject() {
    var defaultBtnValue = $('#submit_btn').html();
    $('#submit_btn').html("Please wait...");
    $('#submit_btn').attr("disabled", true);

    var data = {};
    data.Name = $('#subject_Name').val();
    data.SubjectDescription = $('#subject_Desc').val();
    data.Duration = $('#duration').val();
    if (data.Name != "" && data.SubjectDescription != "") {
        let subjectDetails = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            url: '/SuperAdmin/CreateSubject',
            dataType: 'json',
            data:
            {
                subjectDetails: subjectDetails,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/SuperAdmin/Subject';
                    successAlertWithRedirect(result.msg, url);
                    $('#submit_btn').html(defaultBtnValue);
                }
                else {
                    $('#submit_btn').html(defaultBtnValue);
                    $('#submit_btn').attr("disabled", false);
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                $('#submit_btn').html(defaultBtnValue);
                $('#submit_btn').attr("disabled", false);
                errorAlert("Please check and try again. Contact Admin if issue persists..");
            }
        });
    } else {
        $('#submit_btn').html(defaultBtnValue);
        $('#submit_btn').attr("disabled", false);
        errorAlert("Please fill the form Correctly");
    }
}

function SubjectToBeEdited(id) {
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/SuperAdmin/EditSubject',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                $('#subject_id').val(result.id);
                $('#edit_subject_Name').val(result.name);
                $('#edit_Desc').val(result.subjectDescription);
                $('#edit_duration').val(result.duration);
                $('#edit_subject').modal('show');
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("An error occured, please check and try again. Please contact admin if issue persists..");
        }
    })
}

function SaveEditedSubjet() {
    var defaultBtnValue = $('#submit_Btn').html();
    $('#submit_Btn').html("Please wait...");
    $('#submit_Btn').attr("disabled", true);
    var data = {};
    data.Id = $("#subject_id").val();
    data.Name = $("#edit_subject_Name").val();
    data.SubjectDescription = $("#edit_Desc").val();
    data.Duration = $("#edit_duration").val();
    if (data.Name != "" && data.SubjectDescription != "") {
        let editSubject = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/SuperAdmin/EditedSubject',
            dataType: 'json',
            data:
            {
                editSubject: editSubject,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/SuperAdmin/Subject'
                    successAlertWithRedirect(result.msg, url)
                    $('#submit_Btn').html(defaultBtnValue);
                }
                else {
                    $('#submit_Btn').html(defaultBtnValue);
                    $('#submit_Btn').attr("disabled", false);
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                $('#submit_Btn').html(defaultBtnValue);
                $('#submit_Btn').attr("disabled", false);
                errorAlert(result.msg);
            }
        });
    } else {
        $('#submit_Btn').html(defaultBtnValue);
        $('#submit_Btn').attr("disabled", false);
        errorAlert("Invalid, Please fill the form correctly.");
    }
}

function DeleteSubject() {
    var id = $('#deleteSubject').val();
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/SuperAdmin/DeleteSubject',
        data: {
            id: id
        },
        success: function (result) {
            if (!result.isError) {
                var url = '/SuperAdmin/Subject'
                successAlertWithRedirect(result.msg, url)
                $('#submit_Btn').html(defaultBtnValue);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("An error occured, please check and try again. Please contact admin if issue persists..");
        }
    })
}

function SubjectToDelete(id) {
    $('#deleteSubject').val(id);
    $('#delete_subject').modal('show');
}

function RegisterStaff() {
    var defaultBtnValue = $('#submit_btn').html();
    $('#submit_btn').html("Please wait...");
    $('#submit_btn').attr("disabled", true);

    var data = {};
    data.FirstName = $('#firstname').val();
    data.LastName = $('#lastname').val();
    data.UserName = $('#username').val();
    data.OtherName = $('#othername').val();
    data.Phonenumber = $('#phonenumber').val();
    data.Email = $('#email').val();
    data.Password = $('#password').val();
    data.ConfirmPassword = $('#confirmPassword').val();
    data.State = $('#state').val();
    data.Country = $('#country').val();
    data.Address = $('#address').val();
    data.DOB = $('#dateOfBirth').val();
    
   /* data.Identification = $('#validId').val();*/
    data.SubjectId = $('#subjectId').val();
    if (data.DOB == "") {
        data.DOB = "0001-01-01T00:00:00"
    };
    var appLetter = $('#appLetter').val();
    var StaffPosition;
    var validId = document.getElementById("validId").files;
    if (data.SubjectId > 0) {
        StaffPosition = "";
    } else {
        StaffPosition = $('.user-checkbox').data('user-id');
    }
    if (data.FirstName != "" && data.LastName != "" && data.UserName != "" && data.Phonenumber != ""
        && data.Email != "" && data.Password != "" && data.ConfirmPassword != "" && data.State != "" && data.Country != ""
        && data.Address != "" && validId[0] != null) {
        if (validId[0] != null) {
            const reader = new FileReader();
            reader.readAsDataURL(validId[0]);
            reader.onload = function () {
                validId = reader.result;
                let userDetails = JSON.stringify(data);
                $.ajax({
                    type: 'Post',
                    url: '/Account/StaffRegistration',
                    dataType: 'json',
                    data:
                    {
                        userDetails: userDetails,
                        staffPosition: StaffPosition,
                        appLetter: appLetter,
                        validId: validId
                    },
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Account/Login';
                            successAlertWithRedirect(result.msg, url);
                            $('#submit_btn').html(defaultBtnValue);
                        }
                        else {
                            $('#submit_btn').html(defaultBtnValue);
                            $('#submit_btn').attr("disabled", false);
                            errorAlert(result.msg);
                        }
                    },
                    error: function (ex) {
                        $('#submit_btn').html(defaultBtnValue);
                        $('#submit_btn').attr("disabled", false);
                        errorAlert("Please check and try again. Contact Admin if issue persists..");
                    },
                })

            }
        }
        
    } else {
        $('#submit_btn').html(defaultBtnValue);
        $('#submit_btn').attr("disabled", false);
        errorAlert("Please fill the form Correctly");
    };
}

function viewCoverLetter(id) {
    if (id != null) {
        $.ajax({
            type: 'get',
            dataType: 'json',
            url: '/HumanResource/GetCoverLetter',
            data: {
                Id: id,
            },
            success: function (result) {
                if (!result.isError) {
                    $("#viewCoverLetter").val(result.data.applicationLetter);

                } else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                errorAlert("Network failure, please try again");
            }
        });
    } else {
        errorAlert("Invalid Id");
    }
}

function approveApplication(id) {
    debugger;
    $.ajax({
        type: 'POST',
        url: '/HumanResource/ApproveApplication',
        dataType: 'json',
        data: {
            id: id
        },
        success: function (result) {
            if (!result.isError) {
                var url = '/HumanResource/PendingApplication';
                successAlertWithRedirect(result.msg, url);
                $('#submit_btn').html(defaultBtnValue);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact the support - " + errorAlert(ex);
        }
    });
}

function declineApplication(id) {
    $.ajax({
        type: 'POST',
        url: '/HumanResource/DeclineApplication', // we are calling json method
        dataType: 'json',
        data:
        {
            id: id
        },
        success: function (result) {
            if (!result.isError) {
                var url = '/HumanResource/PendingApplication';
                successAlertWithRedirect(result.msg, url);
                $('#submit_btn').html(defaultBtnValue);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert("Please, Contact the Support for --- " + ex);
        }
    });
}

function viewIDImage(imageUrl) {
    var imageElement = document.getElementById('ImageId');
    imageElement.src = imageUrl;
}

$(document).ready(function () {
    $('#dataTable').DataTable();
});


