// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.onload = _ => {


    Vue.component("facility-item", {

        props: ["item", "updateCallback", "removeCallback"],
        template: ` 
            <tr>
                    <td> {{ item.Id }}                 </td>
                    <td> {{ item.Name }}               </td>
                    <td> {{ item.Address}}             </td>
                    <td> {{ item.PhoneNumber}}         </td>
                    <td> {{ item.Email}}               </td>
                    <td> {{ item.FacilityStatus.Name}} </td>

                    <td> <button v-on:click='removeCallback(item.Id)' class='btn btn-danger'> Delete </button></td>
                    <td> <button v-on:click='updateCallback(item)' class='btn btn-warning'>    Update </button></td>

            </tr> 
 
            `,


    });



    Vue.component('item-form', {

        props: ["item", 'callback', "statuses", "statusDisplay"],

        template:
            `
                <div>
                <form class='form'>
                            <div class='form-group'>
                         <label> Name    <input class='form-control' type="text" v-model:value='item.Name'/>        </label>
                            </div>

                            <div class='form-group'>
                         <label> Address <input class='form-control' type='text' v-model:value='item.Address'/>     </label>
                            </div>

                            <div class='form-group'>
                         <label> Phone   <input class='form-control' type='text' v-model:value='item.PhoneNumber'/> </label>
                            </div>

                            <div class='form-group'>
                         <label> Email   <input class='form-control' type="email"  v-model:value='item.Email'/>     </label>
                            </div>

                
                        <div v-if="statusDisplay" class='form-group'>
                            <label> Status 
                                    <select class='form-control' v-model='item.FacilityStatusId'>
                                        <option v-for="status in statuses" :value='status.Id' > {{ status.Name  }} </option> 
                                    </select>
                            </label>
                        </div>        
              </form>

              <button v-on:click='callback( {
                        FacilityStatusId : item.FacilityStatusId, 
                        Id: item.Id,
                        Name : item.Name,
                        Address : item.Address,
                        PhoneNumber : item.PhoneNumber,
                        Email : item.Email } 
              )'
                      class='btn btn-success'
            > Commit </button>
              </div>
          
                `


    });


    Vue.component('pagination', {
        props: ["pageCount", 'changePageCallback'],
        template: `<ul class='nav'> 
                    <a
                            class='nav-link'
                            onclick = "return false;"
                            v-on:click='changePageCallback( 0 )'
                            href=''> 0 </a>
                    <a
                            class='nav-link'
                            v-for='i in pageCount'
                            onclick = "return false;"
                            v-on:click='changePageCallback( i )'
                            href='' >
                    {{i}}
                    </a>
                     </ul>`,
    });




    Vue.component('facility-list',
        {

            props: ["items", "createCallback", 'updateCallback', 'removeCallback', 'pageCount', 'changePageCallback'],

            data: {
                showUpdate: false,
                updatedItemId: -1,

            },

            template: `<div>

          
            <table class='table thead-dark'>
                 <thead>
                        <tr>
                            <td>Id              </td>
                            <td>Name            </td>
                            <td>Address         </td> 
                            <td>Phone Number    </td> 
                            <td>Email           </td> 
                            <td>FacilityStatus  </td>
                            <td>remove          </td>
                            <td>update          </td> 
                        </tr>
                 </thead>
             

                 <tbody>
                    <facility-item
                                v-for='item in items'
                                :update-callback='updateCallback'
                                :remove-callback='removeCallback'
                                :item='item' ></facility-item>
                  </tbody>

            </table>
            
            <pagination :page-count='pageCount' :change-page-callback='changePageCallback'></pagination>
         
            <button v-on:click='createCallback' class='btn btn-info'> Create </button>
        
             </div>`

        });




    new Vue({
        el: '#app',

        data: {

            pageCount: 0,
            pageItems: [],


            statusesList: [],

            isUpdateNow: false,
            updateItem: {},

            isCreateNow: false,
            currentPage: 0,
        },

        mounted: function () {
            axios
                .get('/Facility/GetCountOfPages')
                .then(response => this.pageCount = response.data);

        
            axios
                .get('/Facility/GetPageItemJson')
                .then(response => this.pageItems = response.data);

            axios
                .get("/Facility/GetFacilityStatuseJson")
                .then(response => this.statusesList = response.data);

        },

        methods: {

            changePage: function (page) {

                this.currentPage = page;


                axios
                    .get('/Facility/GetPageItemJson', {
                        params: {
                            page: page
                        }
                    }) .then(response => this.pageItems = response.data);
            },

            remove: function (id) {
                axios({
                    method: "DELETE",
                    url: "/Facility/Delete/",

                    params: {
                        id: id
                    }
                }).then(_ => this.changePage(this.currentPage));
            },

            update: function (updatedItem) {

                axios({
                    method: "PUT",
                    url: "/Facility/Update/",


                    data: {
                        ...updatedItem
                    }
                }).then(_ => this.changePage(this.currentPage));

                this.isUpdateNow = false;

            },

            showCreateForm: function () {
                this.isCreateNow = true;
            },

            showUpdateForm: function (updateItem) {
                this.updateItem = updateItem;
                this.isUpdateNow = true;
            },

            create: function (data) {
                this.isCreateNow = false;

                axios({
                    method: "POST",
                    url: "/Facility/Create/",

                    data: {
                        ...data
                    }
                }).then(_ => this.changePage(this.currentPage));

            }
        }

    });

};