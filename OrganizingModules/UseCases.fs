/// ===========================================
/// All the use cases or services in one place
/// ===========================================
module UseCases = 

    open CommonLibrary
    open DomainTypes

    let handleUpdateRequest = 
        Validation.combinedValidation 
        >> map Validation.canonicalizeEmail
        >> bind CustomerRepository.updateDatebaseStep
        >> Logger.log
