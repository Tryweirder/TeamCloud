// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import React, { useState } from 'react';
import { CheckboxVisibility, DetailsList, DetailsListLayoutMode, FontWeights, getTheme, IColumn, IDetailsHeaderProps, IDetailsRowProps, IRenderFunction, ITextStyles, Persona, PersonaSize, PrimaryButton, SearchBox, SelectionMode, Stack, Text } from '@fluentui/react';
import { useParams } from 'react-router-dom';
import { Member } from '../model';

export interface IOrgSettingsMembersProps {
    // org?: Organization
    members?: Member[]
}

export const OrgSettingsMembers: React.FunctionComponent<IOrgSettingsMembersProps> = (props) => {

    let { orgId } = useParams() as { orgId: string };

    // const [members, setMembers] = useState<Member[]>();
    const [memberFilter, setMemberFilter] = useState<string>();
    const theme = getTheme();

    const columns: IColumn[] = [
        {
            key: 'member', name: 'Member', minWidth: 240, isResizable: false, onRender: (m: Member) => (
                <Persona
                    text={m.graphUser?.displayName ?? m.user.id}
                    showSecondaryText
                    secondaryText={m.graphUser?.mail ?? (m.graphUser?.otherMails && m.graphUser.otherMails.length > 0 ? m.graphUser.otherMails[0] : undefined)}
                    imageUrl={m.graphUser?.imageUrl}
                    size={PersonaSize.size32} />
            )
        },
        { key: 'role', name: 'Role', minWidth: 240, onRender: (m: Member) => m.user.role },
        { key: 'type', name: 'Type', minWidth: 240, onRender: (m: Member) => m.user.userType }
    ];


    const _applyMemberFilter = (member: Member): boolean => {
        return memberFilter ? member.graphUser?.displayName?.toUpperCase().includes(memberFilter.toUpperCase()) ?? false : true;
    }

    const _onLinkClicked = (member: Member): void => {
        // if (props.onProjectSelected)
        //     props.onProjectSelected(project);
    }

    const _onItemInvoked = (member: Member): void => {
        console.error(member);
        // if (project) {
        //     _onLinkClicked(project);
        //     history.push(`${orgId}/projects/${project.slug}`);
        // } else {
        //     console.error('nope');
        // }
    };

    const _onRenderRow: IRenderFunction<IDetailsRowProps> = (props?: IDetailsRowProps, defaultRender?: (props?: IDetailsRowProps) => JSX.Element | null): JSX.Element | null => {
        if (props) props.styles = { fields: { alignItems: 'center' }, check: { minHeight: '62px' }, cell: { fontSize: '14px' } }
        return defaultRender ? defaultRender(props) : null;
    };

    const _onRenderDetailsHeader: IRenderFunction<IDetailsHeaderProps> = (props?: IDetailsHeaderProps, defaultRender?: (props?: IDetailsHeaderProps) => JSX.Element | null): JSX.Element | null => {
        if (props) props.styles = { root: { paddingTop: '8px' } }
        return defaultRender ? defaultRender(props) : null;
    };

    const items = props.members ? props.members.filter(_applyMemberFilter) : new Array<Member>();

    const _titleStyles: ITextStyles = {
        root: {
            fontSize: '14px',
            fontWeight: FontWeights.regular,
        }
    }

    const _calloutStyles: ITextStyles = {
        root: {
            fontSize: '11px',
            fontWeight: FontWeights.regular,
            color: 'rgb(102, 102, 102)',
            backgroundColor: theme.palette.neutralLighter,
            padding: '2px 9px',
            borderRadius: '14px',
        }
    }


    if (props.members === undefined)
        return (<></>);

    if (props.members.length === 0)
        return (<Text styles={{ root: { width: '100%', paddingLeft: '8px' } }}>No projects</Text>)

    return (
        <Stack tokens={{ childrenGap: '20px' }}>
            <Stack styles={{
                root: {
                    padding: '10px 16px 10px 6px',
                    borderRadius: theme.effects.roundedCorner4,
                    boxShadow: theme.effects.elevation4,
                    backgroundColor: theme.palette.white
                }
            }} >
                <SearchBox
                    placeholder='Filter members'
                    iconProps={{ iconName: 'Filter' }}
                    onChange={(_ev, val) => setMemberFilter(val)}
                    styles={{
                        root: {
                            border: 'none !important', selectors: {
                                '::after': { border: 'none !important' },
                                ':hover .ms-SearchBox-iconContainer': { color: theme.palette.neutralTertiary }
                            }
                        },
                        iconContainer: { color: theme.palette.neutralTertiary, },
                        field: { border: 'none !important' }
                    }} />
            </Stack>
            <Stack styles={{
                root: {
                    borderRadius: theme.effects.roundedCorner4,
                    boxShadow: theme.effects.elevation4,
                    backgroundColor: theme.palette.white
                }
            }} >
                <Stack horizontal verticalFill verticalAlign='baseline' horizontalAlign='space-between'
                    styles={{ root: { padding: '16px 16px 0px 16px', } }}>
                    <Stack.Item>
                        <Stack horizontal verticalFill verticalAlign='baseline' tokens={{ childrenGap: '5px' }}>
                            <Stack.Item>
                                <Text styles={_titleStyles}>Total</Text>
                            </Stack.Item>
                            <Stack.Item>
                                <Text styles={_calloutStyles}>{props.members.length}</Text>
                            </Stack.Item>
                        </Stack>
                    </Stack.Item>
                    <Stack.Item>
                        <PrimaryButton
                            disabled={orgId === undefined}
                            // iconProps={{ iconName: 'Add' }}
                            text='Add members'
                        // onClick={() => history.push(`/orgs/${orgId}/projects/new`)}
                        />
                    </Stack.Item>
                </Stack>

                <DetailsList
                    items={items}
                    columns={columns}
                    // isHeaderVisible={false}
                    onRenderRow={_onRenderRow}
                    onRenderDetailsHeader={_onRenderDetailsHeader}
                    // selectionMode={SelectionMode.none}
                    layoutMode={DetailsListLayoutMode.justified}
                    checkboxVisibility={CheckboxVisibility.always}
                    selectionPreservedOnEmptyClick={true}
                    onItemInvoked={_onItemInvoked} />
            </Stack>
        </Stack>
    );
}