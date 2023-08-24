/*

criando squemas de validascao 
db.createCollection("users", {
   validator: {
      $jsonSchema: {
         bsonType: "object",
         required: [ "username", "password" ],
         properties: {
            username: {
               bsonType: "string",
               description: "must be a string and is required"
            },
            password: {
               bsonType: "string",
               minLength: 8,
               description: "must be a string at least 8 characters long, and is required"
            }
         }
      }
   }
} )

modificando squemas de validacao 


db.runCommand( { collMod: "users",
   validator: {
      $jsonSchema: {
         bsonType: "object",
         required: [ "username", "password" ],
         properties: {
            username: {
               bsonType: "string",
               description: "must be a string and is required"
            },
            password: {
               bsonType: "string",
               minLength: 12,
               description: "must be a string of at least 12 characters, and is required"
            }
         }
      }
   }
} )
*/




use('mongo_poc');




db.createCollection('endereco',{
    validator:{
        $jsonSchema:
        {
                bsonType: 'object',
                required:['logradouro','estado','relationalId'],
                properties:{
                    clienteId:{

                        bsonType:'string',
                        description:'código relacional  do cliente'
                    }, 

                    logradouro:{
                        bsonType:'string',
                        description:'endereço do cliente'

                    },
                    
                    estado:{
                        enum: [ 1, 2, 3 ],
                        description:'estado do endereço'
                    },
                    relationalId:{

                        bsonType:'string',
                        description:'Id da tabela endereco'
                    },
                    cliente:{
                        bsonType:'object',
                        required:['cpf','nome','relationalId'],
                        properties:{
                            cpf:{

                                bsonType: 'string',
                                description: 'CPF da pessoa fisica com 11 digitos sem ponto'
                            },
                            nome:{
    
                                bsonType: 'string',
                                description: ' nome da pessoa'
    
                            },
                            relationalId:{
    
                                bsonType:'string',
                                description:'Id da tabela cliente relacional'
                            }
                        }
                    }
                }

        }
    }

});


db.createCollection('clientes',{
        validator:{
            $jsonSchema:
            {
                    bsonType: 'object',
                    required: ['cpf','nome','relationalId'],
                    properties:{

                        cpf:{

                            bsonType: 'string',
                            description: 'CPF da pessoa fisica com 11 digitos sem ponto'
                        },
                        nome:{

                            bsonType: 'string',
                            description: ' nome da pessoa'

                        },
                        relationalId:{

                            bsonType:'string',
                            description:'Id da tabela cliente relacional'
                        },
                        enderecos:{
                            bsonType:'array',
                            required:['logradouro','estado','relationalId'],
                            properties:{
                                    logradouro:{
                                        bsonType:'string',
                                        description:'endereço do cliente'

                                    },
                                    
                                    estado:{
                                        enum: [ 1, 2, 3 ],
                                        description:'estado do endereço'
                                    },
                                    relationalId:{

                                        bsonType:'string',
                                        description:'Id da tabela endereco'
                                    }

                            }
                        }
                    }

            }
        }
});


db.createCollection('pedidoitens', {
    validator: {
       $jsonSchema: {
          bsonType: "object",
          required:['relationalId'],
          properties:{
              qtd:{
                  bsonType:'int',
                  description:'quantidade de itens'
              },
              relationalId:{

                bsonType:'string',
                description:'Id da tabela cliente relacional'
            },
              price:{
                  bsonType:'string',
                  description:'valor unitário'
              },
              produtoId:{
                  bsonType:'string',
                  description:'código do produto'
              },
              produto:{
                  bsonType: "object",
                  required: [ "descricao"],
                  properties:{
                      descricao:{
                          bsonType:'string',
                          description:'nome Produto'
                      }
                  }
              }
          }
       }
    }
 })

 db.createCollection('notaitens', {
    validator: {
       $jsonSchema: {
          bsonType: "object",
          required:['relationalId'],
          properties:{
            relationalId:{

                bsonType:'string',
                description:'Id da tabela cliente relacional'
            },

              qtd:{
                  bsonType:'int',
                  description:'quantidade de itens'
              },
              price:{
                  bsonType:'string',
                  description:'valor unitário'
              },
              produtoId:{
                  bsonType:'string',
                  description:'código do produto'
              },
              produto:{
                  bsonType: "object",
                  required: [ "descricao"],
                  properties:{
                      descricao:{
                          bsonType:'string',
                          description:'nome Produto'
                      }
                  }
              }
          }
       }
    }
 })


 db.createCollection('nota', {
    validator: {
       $jsonSchema: {
          bsonType: "object",
          required: [ "fornecedorId","clienteId","numero","notaItens","relationalId" ],
          properties: {
            observacao:{
                bsonType:'string',
                description:'Observação da Nota'                
            },
            numero:{
                bsonType:"string",
                description:"número da Nota"
            },
           relationalId:{

                            bsonType:'string',
                            description:'Id da tabela cliente relacional'
                        },
            notaItens:{
                bsonType:'array',
                required:['qtd','price','produtoId'],
                properties:{
                    qtd:{
                        bsonType:'int',
                        description:'quantidade de itens'
                    },
                    price:{
                        bsonType:'decimal',
                        description:'valor unitário'
                    },
                    produtoId:{
                        bsonType:'string',
                        description:'código do produto'
                    },
                    produto:{
                        bsonType: "object",
                        required: [ "descricao"],
                        properties:{
                            descricao:{
                                bsonType:'string',
                                description:'nome Produto'
                            }
                        }
                    }
                }
            },
            clienteId:{
                bsonType:"string",
                description:"Código do cliente"    
            },
            cliente:{
                bsonType: "object",
                required: [ "cpf","nome" ],
                properties:{

                      cpf:{
                        bsonType: "string",
                        description: "documento do cliente"
                      },
                      nome:{
                        bsonType: "string",
                        description: "nome do cliente"
                      }      
                    }    
            },
            fornecedorId: {
                bsonType: "string",
                description: "Id do fornecedor"
             },
             fornecedor:{
                bsonType: "object",
                required: [ "cnpj","razaoSocial" ],
                properties: {
                    cnpj:{
                        bsonType: "string",
                        description: "documento do fornecedor"
                    },
                    razaoSocial:{
                        bsonType: "string",
                        description: "nome do Fornecedor"
                    }
                }

             }
          }
       }
    }
 })

db.createCollection('pedido', {
    validator: {
       $jsonSchema: {
          bsonType: "object",
          required: [ "fornecedorId","clienteId","pedidoItens", "relationalId" ],
          properties: {
            observacao:{
                bsonType:'string',
                description:'Observação da Nota'                
            },
 relationalId:{

                            bsonType:'string',
                            description:'Id da tabela cliente relacional'
                        },
	   
            pedidoItens:{
                bsonType:'array',
                required:['qtd','price','produtoId'],
                properties:{
                    qtd:{
                        bsonType:'int',
                        description:'quantidade de itens'
                    },
                    price:{
                        bsonType:'decimal',
                        description:'valor unitário'
                    },
                    produtoId:{
                        bsonType:'string',
                        description:'código do produto'
                    },
                    produto:{
                        bsonType: "object",
                        required: [ "descricao"],
                        properties:{
                            descricao:{
                                bsonType:'string',
                                description:'nome Produto'
                            }
                        }
                    }
                }
            },
            clienteId:{
                bsonType:"string",
                description:"Código do cliente"    
            },
            cliente:{
                bsonType: "object",
                required: [ "cpf","nome" ],
                properties:{

                      cpf:{
                        bsonType: "string",
                        description: "documento do cliente"
                      },
                      nome:{
                        bsonType: "string",
                        description: "nome do cliente"
                      }      
                    }    
            },
            fornecedorId: {
                bsonType: "string",
                description: "Id do fornecedor"
             },
             fornecedor:{
                bsonType: "object",
                required: [ "cnpj","razaoSocial" ],
                properties: {
                    cnpj:{
                        bsonType: "string",
                        description: "documento do fornecedor"
                    },
                    razaoSocial:{
                        bsonType: "string",
                        description: "nome do Fornecedor"
                    }
                }

             }
          }
       }
    }
 })

 db.createCollection('produto', {
    validator: {
       $jsonSchema: {
          bsonType: "object",
          required: [ "descricao" ],
          properties: {
             descricao: {
                bsonType: "string",
                description: "descricao produto"
             }
          }
       }
    }
 })

 db.createCollection('fornecedor',{

    validator:{

        $jsonSchema:{
            bsonType: 'object',
            required:['cnpj','razaoSocial','relationalId'],
            properties:{
            relationalId:{
                bsonType:'string',
                description:'Id da tabela fornecedor'
            },
            cnpj:{
                  bsonType:'string',
                  description:'CNPJ documento da empresa'      

            },
            razaoSocial:{
                   bsonType:'string',
                   description:'nome da empresa'      

            }
        }

        }
    }

})




