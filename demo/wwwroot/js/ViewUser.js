function init() {
    return {
        showForm: false,
        ViewUser: {

            Term: "",
            amount: "",
            type: "",

        },


        initDatabase() {

        },
        
    //    documentquerySelectorAll('[data-toggle-form]').forEach(function (button) {
    //        button.addEventListener('click', function () {
    //            document.getElementById('contactForm').dispatchEvent(new Event('toggleFormVisibility'));
    //        });
    //    });

    //}
            
           
            
            var self = this;
            var getUser = {
                url: "/api/FetchAmount?id=" + 7,
                method: "GET"
            };

            $.ajax(getUser)
                .done(function (data) {

                    self.TermId = data.id;

                    self.ViewUser.term = data.term;
                    self.ViewUser.amount = data.amount;
                })
                .fail(function (xhr, status, error) {
                    console.error(xhr.responseText);
                    // Handle error scenarios
                });
        }
    
    

