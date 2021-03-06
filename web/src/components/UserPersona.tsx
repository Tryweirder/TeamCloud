// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import React from 'react';
import { IPersonaStyleProps, IPersonaStyles, IStyleFunctionOrObject, Persona, PersonaSize } from '@fluentui/react';
import { GraphUser } from '../model';

export interface IUserPersonaProps {
    size?: PersonaSize;
    user?: GraphUser;
    large?: boolean;
    showSecondaryText?: boolean;
    hidePersonaDetails?: boolean;
    onClick?: () => void;
    styles?: IStyleFunctionOrObject<IPersonaStyleProps, IPersonaStyles>;
}

export const UserPersona: React.FunctionComponent<IUserPersonaProps> = (props) => {

    const text = props.user?.displayName;

    const mail = props.user?.mail ?? (props.user?.otherMails?.length ?? 0) > 0 ? props.user!.otherMails![0] : props.user?.userPrincipalName;

    const secondaryText = props.large ? props.user?.jobTitle : mail;

    const tertiaryText = props.large ? props.user?.department : undefined;

    const imageUrl = props.user?.imageUrl;

    return (
        <Persona
            hidePersonaDetails={props.hidePersonaDetails}
            showSecondaryText={props.showSecondaryText}
            text={text}
            secondaryText={secondaryText}
            tertiaryText={tertiaryText}
            imageUrl={imageUrl}
            styles={props.styles}
            onClick={props.onClick}
            size={props.size ? props.size : (props.large ? PersonaSize.size72 : PersonaSize.size32)} />
    );
}
