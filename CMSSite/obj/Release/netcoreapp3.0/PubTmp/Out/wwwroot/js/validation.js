$(document).ready(function () {



    // Wait for the DOM to be ready
    $(function () {
        // Initialize form validation on the registration form.
        // It has the name attribute "registration"
        if ($("#contact_form").length > 0) {
            var langID = $("html").attr("lang");
            $("#contact_form").validate({
                // Specify validation rules
                rules: {

                    systemName: {
                        required: {
                            depends: function (element) {
                                return $('#systemName').length !== 0;
                            }
                        }
                    },
                    email: {
                        required: {
                            depends: function (element) {
                                return $('#email').length !== 0;
                            }
                        },
                        email: true
                    },
                    emailR: {
                        required: {
                            depends: function (element) {
                                return $('#emailR').length !== 0;
                            }
                        },
                        email: true,
                        equalTo: "#email"
                    },
                    name: {
                        required: {
                            depends: function (element) {
                                return $('#name').length !== 0;
                            }
                        }
                    }
                    ,
                    surname: {
                        required: {
                            depends: function (element) {
                                return $('#surname').length !== 0;
                            }
                        }
                    }
                    ,
                    firmName: {
                        required: {
                            depends: function (element) {
                                return $('#firmName').length !== 0;
                            }
                        }
                    }
                    ,
                    mobilePhone: {
                        required: {
                            depends: function (element) {
                                return $('#mobilePhone').length !== 0;
                            }
                        }
                    }
                    ,
                    rSubject: {
                        required: {
                            depends: function (element) {
                                return $('#rSubject').length !== 0;
                            }
                        }
                    }
                    ,
                    CheckPolicy: {
                        required: true
                    },
                    departman: {
                        required: {
                            depends: function (element) {
                                return $('#departman').length !== 0;
                            }
                        }
                    }
                    ,
                    subjectH: {
                        required: {
                            depends: function (element) {
                                return $('#subjectH').length !== 0;
                            }
                        }
                    }
                    ,
                    Checks: {
                        required: {
                            depends: function (element) {
                                return $('#Checks').length !== 0;
                            }
                        }
                    }
                },
                messages: {
                   
                },
                submitHandler: function (form) {
                    if ($("input:checked").length > 0) {
                        $("#mail_Check").hide();
                        $("#CheckPolicy").removeClass("error");
                        $('#send_message').attr({ 'disabled': 'true', 'value': (langID=='en'?'Sending...':'Gönderiliyor...') }); 
                        $('#send_message').html((langID == 'en' ? 'Sending...' : 'Gönderiliyor...'));

                        // var dataFile = new FormData();
                        // $.each($('#file')[0].files, function (key, value) {
                        //     dataFile.append(key, value);
                        // });
                        // var data = {
                        //     'files': dataFile,
                        //     'form': $(form).serialize()
                        // }
                        var formData = new FormData($(form)[0]);
                        $.ajax({
                            type: "POST",
                            url: "/Base/SendMail",
                            data: formData,
                            cache: false,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                if (result == 'sent') {
                                    $('#submit').remove();
                                    $('#mail_success').fadeIn(500);
                                } else {
                               
                                    $('#send_message').removeAttr('disabled').attr('value', 'Gönder').html((langID == 'en' ? 'Send' : 'Gönder'));
                                    if (result == "SizeEx") {
                                        $('#mail_fail').fadeIn(500);
                                        $('#mail_fail').html((langID == 'en' ? '(jpg|jpeg|png|gif|zip|doc|docx|pdf) type and maximum 5 MB file can be upload.' : '(jpg|jpeg|png|gif|zip|doc|docx|pdf) türünde ve boyut olarak 5 MB dosya yüklenmelidir.'))
                                    }
                                }
                            },
                            error: function (response, error) {
                                console.log(error);
                                console.log(response);
                                $('#send_message').attr({ 'disabled': 'true', 'value': (langID == 'en' ? 'Error, please try again later.' : 'Hata oluştu, lütfen daha sonra tekrar deneyiniz.') });

                            }
                        })

                    }
                    else {
                        $("#mail_Check").show();
                    }
                    //   form.submit();
                }
            });
        }
    });

});